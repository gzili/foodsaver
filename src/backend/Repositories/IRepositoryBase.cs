using System.Linq;

namespace backend.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> Items { get; }
        void Create(T entity);
    }
}