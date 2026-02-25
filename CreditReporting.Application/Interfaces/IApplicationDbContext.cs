using CreditReporting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CreditReporting.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<CibilReport> CibilReports { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
