using System.Collections.Generic;

namespace Demo.Performace.Acesso.Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        int Insert(T item);
        void Remove(T item);
        void Update(T item);
        T GetBy(int id);
        List<T> GetAll();
    }
}