using System;
using backend.Repositories;

namespace backend.Services
{
    public static class DataLoadingService
    {
        private const string DbListsPath = "data/dblists.json";

        public static DbLists LoadDbLists()
        {
            return FileService<DbLists>.ReadJson(DbListsPath);
        }
        
        public static void SaveDbLists(DbLists dbLists)
        {
            FileService<DbLists>.WriteJsonWithReferences(DbListsPath, dbLists);
        }
    }
}