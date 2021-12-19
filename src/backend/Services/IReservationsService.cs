using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public interface IReservationsService
    {
        void Create(Reservation reservation);
        Reservation FindById(int id);
        IEnumerable<Offer> GetReservedOffersByUserId(int userId);
        void Complete(Reservation reservation);
        void Delete(Reservation reservation);
    }
}