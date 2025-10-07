using InfrastructureLayer.BankSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.InfrastructureBases
{
    public class Repository<T> :IRepository<T> where T : class
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion
        #region Constructor

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion
        #region Methods
        public virtual async Task<T> GetByIdAsync(int id)
        {

            return await _context.Set<T>().FindAsync(id);
        }


        public IQueryable<T> GetTableNoTracking()
        {
            return _context.Set<T>().AsNoTracking().AsQueryable();
        }


        public virtual async Task AddRangeAsync(ICollection<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();

        }
        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task DeleteRangeAsync(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public IDbContextTransaction BeginTransaction()
        {


            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();

        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();

        }

        public IQueryable<T> GetTableAsTracking()
        {
            return _context.Set<T>().AsQueryable();

        }

        public virtual async Task UpdateRangeAsync(ICollection<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }


        #endregion





    }
}
