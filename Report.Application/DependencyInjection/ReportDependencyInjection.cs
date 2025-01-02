using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Application.Services.Abstract;
using Report.Application.Services.Concrete;
using Report.Infrastructure.Context;

namespace Report.Application.DependencyInjection
{
    public static class ReportDependencyInjection
    {
        public static IServiceCollection AddReportConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("ReportDevDb"),
                b => b.MigrationsAssembly("StayScanner.Api")));

            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IRabbitMqService, RabbitMqService>();
            return services;
        }
    }

}
