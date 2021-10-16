using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public interface IService<T> where T : EntityModel
    {
        public T GetById(int id);
        public List<T> GetAll();
        public void Save(T t);
    }
}