using Hotel.Application.Services.Abstract;
using Hotel.Application.Services.Concrete;
using Hotel.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Hotel.Application.DependencyInjection
{
    public static class HotelDependencyInjection
    {
        public static IServiceCollection AddHotelConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<HotelDbContext>(opt =>
                    opt.UseNpgsql(configuration.GetConnectionString("HotelDevDb"),
                    b =>
                    b.MigrationsAssembly("StayScanner.Api")));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<HotelDbContext>();
                
                var retryCount = 5;
                var delay = TimeSpan.FromSeconds(5);
                for (int i = 0; i < retryCount; i++)
                {
                    try
                    {
                        dbContext.Database.Migrate();
                        break;
                    }
                    catch (NpgsqlException ex)
                    {
                        if (i == retryCount - 1)
                            throw;

                        Thread.Sleep(delay);
                    }
                }
            }
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IContactService, ContactService>();

            return services;
        }
    }
}
