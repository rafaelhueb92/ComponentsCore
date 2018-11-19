using Components.Entities;
using System;
using System.Collections.Generic;

namespace Components.Contracts
{
    public interface IRepository<T> : IDisposable
       where T : Entity
    {

        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        T Get(object pk);
        IEnumerable<T> Get();

    }
}
