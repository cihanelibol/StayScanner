using Microsoft.EntityFrameworkCore;

namespace Report.Infrastructure.Context
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options):base(options) 
        {
            
        }
        public DbSet<Domain.Entities.Report> Reports { get; set; }
    }
}
