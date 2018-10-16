using GamesInfo.API.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamesInfo.API.Services
{
    internal class GamesFetcher : IHostedService
    {
        private Hashtable games = new Hashtable();
        private readonly ILogger<GamesFetcher>  _logger;
        private readonly IGameInfoRepository _repository;
        private readonly IFetcherAlgorithm _fetcher;
        private Timer _timer;

        public GamesFetcher(ILogger<GamesFetcher> logger, IGameInfoRepository repository, IFetcherAlgorithm fetcher)
        {
            _logger = logger;
            _repository = repository;
            _fetcher = fetcher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            double refetchTimeInSec = Convert.ToDouble(Startup.Configuration["refetchTimeInSec"]);
            _timer = new Timer(FetchGames, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(refetchTimeInSec));

            return Task.CompletedTask;
        }

        private void FetchGames(object state)
        {
             var json = _fetcher.Fetch();

            JObject o = JObject.Parse(json); // parse as array  

            var matches = o["data"]["match"];

             GamesCollection.ClearWorking();

 
            // Go over return value from the site,
            foreach(var match in matches)
            {
                //Create Game Key
                StringBuilder gameKey = new StringBuilder(50);
                gameKey.Append(match["home_name"] + "_" + match["away_name"] + "_" + match["league_name"]);
                Game game = new Game
                {
                    Home = match["home_name"].ToString(),
                    Away = match["away_name"].ToString(),
                    League = match["league_name"].ToString(),
                    Key = gameKey.ToString()
                };
   
                try
                {
                    // Add into memory Hash
                    GamesCollection.AddGame(game);

                    // Immediately adds a game to mongo for consistency
                    _repository.Create(game);

                }
                catch(Exception e)
                {
                    //Remove from Collection
                    GamesCollection.RemoveGame(game.Key);
                    _logger.LogError(e.Message);
                }

                //Log
                _logger.LogInformation(match.ToString());
            }

            // Now get the games that are not longer in the site.
            var listGamesToDelete = GamesCollection.GetToDeleteGames();
            // and delete the from DB
            _repository.DeleteBulk(listGamesToDelete);

            //swap collections so the reading will be from a collection that is not being changed.
            GamesCollection.SwapCollections();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
