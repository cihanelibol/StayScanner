﻿using Hotel.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Application.DependencyInjection
{
    public static class HotelDependencyInjection
    {
        public static IServiceCollection AddHotelConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("HotelDevDb"),
                 b =>
                 b.MigrationsAssembly("StayScanner.Api")));

            return services;
        }
    }
}