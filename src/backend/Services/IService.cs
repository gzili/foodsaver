using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public interface IService<T> where T : EntityModel
    {
        T GetById(int id);
        List<T> GetAll();
        void Save(T t);
    }
}