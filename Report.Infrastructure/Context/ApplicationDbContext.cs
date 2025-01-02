using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities;

namespace Report.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
        public DbSet<HotelsByLocation> HotelsByLocations { get; set; }
    }
}
