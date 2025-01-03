using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Context
{
    public class HotelDbContext :DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options):base(options) 
        {
            
        }
        public DbSet<Hotel.Domain.Entities.Hotel> Hotels { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
