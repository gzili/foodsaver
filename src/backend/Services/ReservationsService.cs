using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class ReservationsService
    {
        private readonly ReservationsRepository _reservationsRepository;

        public ReservationsService(ReservationsRepository reservationsRepository)
        {
            _reservationsRepository = reservationsRepository;
        }
        
        public void Save(Reservation reservation)
        {
            _reservationsRepository.Save(reservation);
        }
        
        public void Delete(Reservation reservation)
        {
            _reservationsRepository.Delete(reservation);
        }

        public Reservation GetById(int id)
        {
            return _reservationsRepository.GetById(id);
        }

        public List<Reservation> GetAll()
        {
            return _reservationsRepository.GetAll();
        }
    }
}