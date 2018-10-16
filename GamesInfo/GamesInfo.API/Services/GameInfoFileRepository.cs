using GamesInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesInfo.API.Services
{
    public class GameInfoFileRepository : IGameInfoRepository
    {
        public Task Create(Game game)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string Key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBulk(List<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Game>> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public Task<Game> GetGame(string Key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Game game)
        {
            throw new NotImplementedException();
        }
    }

};

