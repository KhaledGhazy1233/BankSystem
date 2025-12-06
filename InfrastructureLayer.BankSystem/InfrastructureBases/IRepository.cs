using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace InfrastructureLayer.BankSystem.InfrastructureBases
{
    public interface IRepository<T> where T : class
    {
        Task DeleteRangeAsync(ICollection<T> entities);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        // Remove Operations (Soft Delete) - // الجديدة
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // Additional Useful Methods
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync(Expression<Func<T, bool>> expression = null);

        // Pagination - // الجديدة
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


    }
}
