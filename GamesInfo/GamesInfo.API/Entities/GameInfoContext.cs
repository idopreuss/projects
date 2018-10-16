
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamesInfo.API.Entities
{
    public class GameInfoContext : IGameInfoContext
    {
        private readonly IMongoDatabase _db;
        public GameInfoContext(ILogger<GameInfoContext> logger)
        {
            var client = new MongoClient(Startup.Configuration["MongoDB:ConnectionString"]);
            _db = client.GetDatabase(Startup.Configuration["MongoDB:Database"]);

            // Create Collection if does not exist
            if(!CollectionExists("Games"))
                _db.CreateCollection("Games");

        }

        public IMongoCollection<Game> Games => _db.GetCollection<Game>("Games");

        public bool  CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = _db.ListCollections(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return collections.Any();
        }
    }
}
