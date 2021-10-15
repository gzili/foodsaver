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
            //if _appDbContext is null, returns new, if not null, returns _appDbContext
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