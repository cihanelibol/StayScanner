using Microsoft.EntityFrameworkCore;

namespace Report.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
        public DbSet<Domain.Entities.Report> Reports { get; set; }
    }
}
