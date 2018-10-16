using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesInfo.API.Entities;
using MongoDB.Driver;

namespace GamesInfo.API.Services
{
    public class GameInfoDBRepository : IGameInfoRepository
    {
        private readonly IGameInfoContext _context;
        public GameInfoDBRepository(IGameInfoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _context
                            .Games
                            .Find(_ => true)
                            .ToListAsync();
        }

        public async Task<Game> GetGame(string Key)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq(m => m.Key, Key);
            return await _context
                    .Games
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }

        public async Task Create(Game game)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq(m => m.Key, game.Key);
            if((await _context
                    .Games
                    .Find(filter).CountDocumentsAsync() == 0))
                        await _context.Games.InsertOneAsync(game);
        }
        public async Task<bool> Update(Game game)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Games
                        .ReplaceOneAsync(
                            filter: g => g.Id == game.Id,
                            replacement: game);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string Key)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq(m => m.Key, Key);
            DeleteResult deleteResult = await _context
                                                .Games
                                                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteBulk(List<string> keys)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.In(m => m.Key, keys);
            DeleteResult deleteResult = await _context
                                               .Games
                                               .DeleteManyAsync(filter);
            return deleteResult.IsAcknowledged
            && deleteResult.DeletedCount > 0;

        }

    }
}
