using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.BankSystem.ImplementRepositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<AuditLog> auditlog;
        public AuditLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            auditlog = _context.Set<AuditLog>();
        }

        public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
        {
            return await auditlog
                .Include(a => a.User)
                .AsNoTracking() // أداء أسرع لأننا بنقرأ فقط
                .FirstOrDefaultAsync(a => a.Id == id && !a.ISDeleted);
        }

        public async Task<(List<AuditLog> Items, int TotalCount)> GetUserActivityAsync(
         int userId, int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            var query = auditlog
                .AsNoTracking()
                .Where(a => a.UserId == userId && !a.ISDeleted);

            // فلترة التاريخ لو المطور بعتها
            if (fromDate.HasValue) query = query.Where(a => a.DateTime >= fromDate.Value);
            if (toDate.HasValue) query = query.Where(a => a.DateTime <= toDate.Value);

            // بنحسب الإجمالي قبل ما نقطع الداتا بالـ Pagination
            var totalCount = await query.CountAsync();

            // بنجيب "الشويه" اللي عليهم الدور في العرض بس
            var items = await query
                .OrderByDescending(a => a.DateTime) // الأحدث فوق
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount); // بنرجع الداتا والعدد الكلي في Tuple
        }
    }
}
