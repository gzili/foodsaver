using backend.Services;

namespace backend.Repositories
{
    public class AppDbContext
    {
        private static AppDbContext _appDbContext;

        private AppDbContext()
        {
            Initialize();
        }

        public static AppDbContext GetObject()
        {
            // if creates new AppDbContext if null, else returns existing
            return _appDbContext ??= new AppDbContext();
        }

        public DbLists DbLists { get; set; }

        public object this[string fieldName]
        {
            get
            {
                return fieldName switch
                {
                    "offers" => DbLists.Offers,
                    "users" => DbLists.Users,
                    "foods" => DbLists.Foods,
                    _ => null
                };
            }
        }

        private void Initialize()
        {
            DbLists = DataLoadingService.LoadDbLists();
        }
    }
}