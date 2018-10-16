using GamesInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesInfo.API.Services
{
    public interface IGameInfoRepository
    {
        Task <IEnumerable<Game>> GetAllGames();
        Task <Game> GetGame(string Key);
        Task Create(Game game);
        Task <bool> Update(Game game);
        Task <bool> Delete(string Key);
        Task <bool> DeleteBulk(List<string> keys);
    }
}
