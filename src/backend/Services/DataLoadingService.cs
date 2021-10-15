using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public static class DataLoadingService
    {
        private const string DbListsPath = "wwwroot/dblists.json";

        public static DbLists LoadDbLists()
        {
            return FileService<DbLists>.ReadJson(DbListsPath);
        }
        
        public static void SaveDbLists(DbLists dbLists)
        {
            FileService<DbLists>.WriteJson(DbListsPath, dbLists);
        }
    }
}