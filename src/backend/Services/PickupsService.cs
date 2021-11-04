using System.Collections.Generic;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class PickupsService
    {
        private readonly PickupsRepository _pickupsRepository;

        public PickupsService(PickupsRepository pickupsRepository)
        {
            _pickupsRepository = pickupsRepository;
        }
        
        public void Save(Pickup pickup)
        {
            _pickupsRepository.Save(pickup);
        }

        public Pickup GetById(int id)
        {
            return _pickupsRepository.GetById(id);
        }

        public List<Pickup> GetAll()
        {
            return _pickupsRepository.GetAll();
        }
    }
}