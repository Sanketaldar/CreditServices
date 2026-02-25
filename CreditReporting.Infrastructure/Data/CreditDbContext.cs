using CreditReporting.Application.Interfaces;
using CreditReporting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditReporting.Infrastructure.Data
{
    public class CreditDbContext : DbContext, IApplicationDbContext
    {
        public CreditDbContext(DbContextOptions<CreditDbContext> options) : base(options)
        {
        }

        public DbSet<CibilReport> CibilReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CibilReport>(entity =>
            {
                entity.HasKey(e => e.CibilId);
                entity.Property(e => e.PanNo).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("success");
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.DeletedBy).HasMaxLength(50);
            });
        }
    }
}
