using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Report.Application.BackgroundServices;
using Report.Application.Message.Consumer;
using Report.Application.Services.Abstract;
using Report.Application.Services.Concrete;
using Report.Infrastructure.Context;

namespace Report.Application.DependencyInjection
{
    public static class ReportDependencyInjection
    {
        public static IServiceCollection AddReportConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReportDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("ReportDevDb"),
                b => b.MigrationsAssembly("StayScanner.Api")));


            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<ReportDbContext>();
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

            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IRabbitMqService, RabbitMqService>();
            services.AddHostedService<GetHotelByLocationConsumer>();
            services.AddHostedService<HotelsInfoByLocationReportCreator>();

            return services;
        }
    }

}
