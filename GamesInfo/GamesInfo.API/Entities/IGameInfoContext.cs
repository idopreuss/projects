using GamesInfo.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesInfo.API.Entities
{
    public interface IGameInfoContext 
    {
        IMongoCollection<Game> Games { get; }
    }
}
