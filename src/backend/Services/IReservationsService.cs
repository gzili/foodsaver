using backend.Models;

namespace backend.Services
{
    public interface IReservationsService
    {
        void Create(Reservation reservation);
        Reservation FindById(int id);
        void Complete(Reservation reservation);
        void Delete(Reservation reservation);
    }
}