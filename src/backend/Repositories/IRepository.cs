using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public interface IRepository <T> where T : EntityModel
    {
        void Save(T t);
        T GetById(int id);
        List<T> GetAll();
    }
}