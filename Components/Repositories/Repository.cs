using Components.Contracts;
using Components.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Components.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : Entity
    {

        private readonly DbContext _ctx;

        public Repository(DbContext ctx)
        {
            _ctx = ctx;
        }

        public T Add(T entity)
        {
            _ctx.Set<T>().Add(entity);
            save();
            return entity;
        }

        public void Delete(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            save();
        }

        public T Get(object pk)
        {
            return _ctx.Set<T>().Find(pk);
        }

        public IEnumerable<T> Get()
        {
            return _ctx.Set<T>()
                .AsNoTracking() //desativa o dynamic proxie
                .ToList();
        }

        public void Update(T entity)
        {
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            save();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        private void save()
        {
            _ctx.SaveChanges();
        }
    }
}
