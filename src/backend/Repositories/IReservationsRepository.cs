using backend.Models;

namespace backend.Repositories
{
    public interface IReservationsRepository : IRepositoryBase<Reservation>
    {
        void Delete(Reservation reservation);
    }
}