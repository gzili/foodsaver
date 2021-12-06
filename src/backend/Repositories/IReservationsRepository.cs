using backend.Models;

namespace backend.Repositories
{
    public interface IReservationsRepository : IRepositoryBase<Reservation>
    {
        void Create(Reservation reservation);
        void Delete(Reservation reservation);
    }
}