using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                dbContext.Database.Migrate(); 
            }

            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IRabbitMqService, RabbitMqService>();
            services.AddHostedService<GetHotelByLocationConsumer>();
            services.AddHostedService<HotelsInfoByLocationReportCreator>();

            return services;
        }
    }

}
