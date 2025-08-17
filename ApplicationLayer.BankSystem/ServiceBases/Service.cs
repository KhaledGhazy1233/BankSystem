using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ServiceBases
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> Repository;

        public Service(IRepository<T> repository)
        {

            Repository = repository;
        }

        public virtual Task<T> AddAsync(T entity)
        {
            return Repository.AddAsync(entity);
        }

        public virtual Task AddRangeAsync(ICollection<T> entities)
        {
            return Repository.AddRangeAsync(entities);
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            return Repository.BeginTransaction();
        }

        public virtual void Commit()
        {
            Repository.Commit();

        }

        public virtual Task DeleteAsync(T entity)
        {
            return Repository.DeleteAsync(entity);
        }

        public virtual Task DeleteRangeAsync(ICollection<T> entities)
        {
            return Repository.DeleteRangeAsync(entities);
        }

        public virtual Task<T> GetByIdAsync(int id)
        {
            return Repository.GetByIdAsync(id);

        }

        public virtual IQueryable<T> GetTableAsTracking()
        {
            return Repository.GetTableAsTracking();
        }

        public virtual IQueryable<T> GetTableNoTracking()
        {
            return Repository.GetTableNoTracking();
        }

        public void RollBack()
        {
            Repository.RollBack();
        }

        public virtual Task SaveChangesAsync()
        {
            return Repository.SaveChangesAsync();
        }

        public virtual Task UpdateAsync(T entity)
        {
            return Repository.UpdateAsync(entity);
        }

        public virtual Task UpdateRangeAsync(ICollection<T> entities)
        {
            return Repository.UpdateRangeAsync(entities);
        }
    }
}
