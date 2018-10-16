using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamesInfo.API.Entities
{
    public static class GamesCollection
    {
        private static Hashtable games1 = new Hashtable();
        private static Hashtable games2 = new Hashtable();
        private static Hashtable gamesCollectionForWrite = games1;
        private static Hashtable gamesCollectionForRead = games2;
        private static List<string> gamesToDelete = new List<string>();
        private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim(); //Read Write Lock

        public static List<Game> GetAllGames()
        {
            List<Game> list = new List<Game>();
            locker.EnterReadLock();
            try
            {
                foreach (DictionaryEntry entry in gamesCollectionForRead)
                {
                    list.Add((Game)entry.Value);
                }
            }
            finally
            {
                locker.ExitReadLock();
            }
            return list;
        }
        public static Game GetGame(string Key)
        {
            Game game = null;
            locker.EnterReadLock();
            try
            {
                if (gamesCollectionForRead.ContainsKey(Key))
                {
                    game = (Game)gamesCollectionForRead[Key];
                }
            }
            finally
            {
                locker.ExitReadLock();
            }
            return game;

        }
        public static void AddGame(Game game)
        {
            locker.EnterWriteLock();
            try
            {
                if (!gamesCollectionForWrite.ContainsKey(game.Key))
                {
                    gamesCollectionForWrite.Add(game.Key, game);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
        public static void RemoveGame(string Key)
        {
            locker.EnterWriteLock();
            try
            {
                if (gamesCollectionForWrite[Key] != null)
                {
                    gamesCollectionForWrite.Remove(Key);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public static void SwapCollections()
        {
            locker.EnterWriteLock();
            try
            {
               if (gamesCollectionForWrite == games1)
                {
                    gamesCollectionForWrite = games2;
                    gamesCollectionForRead = games1;
                }
                else
                {
                    gamesCollectionForWrite = games1;
                    gamesCollectionForRead = games2;
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public static void ClearWorking()
        {
            // Prepare for next round
            gamesCollectionForWrite.Clear();
        }
        public static List<string> GetToDeleteGames()
        {
            //Go over updated. for each one, look for the key in the working hash. if not found, add to deletion.
            foreach(DictionaryEntry previousGame in gamesCollectionForRead)
            {
                if(!gamesCollectionForWrite.ContainsKey(previousGame.Key))
                {
                    gamesToDelete.Add((string)previousGame.Key);
                }
            }

            return gamesToDelete;


        }
    }
}   
