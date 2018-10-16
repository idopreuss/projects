using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesInfo.API.Entities;
using GamesInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace GamesInfo.API.Controllers
{
    [Route("api/games")]
    public class GamesInfoController : Controller
    {
        private ILogger<GamesInfoController> _logger;
        private readonly IGameInfoRepository _gameRepository;

        public GamesInfoController (ILogger<GamesInfoController> logger, IGameInfoRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }
        
        // In this controller i have decieded to retreive games from the in memort hash. we can easily make it go to DB by using our repository.

        [HttpGet()]
        public IActionResult GetGames()
        {
            return Ok(GamesCollection.GetAllGames());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}