using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Context;
using MyFinance.Infra.Repositories;
using System;

namespace MyFinance.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServicesInfra(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddTransient<IUserRepository, UserRepository>();
          
            services.AddTransient<IUnitOfWork, UnitOfWork>();
           services.AddDbContext<MyfinancesContext>(opt => opt.UseSqlServer("Data Source=DESKTOP-44UF9P5;Initial Catalog=MYFINANCES;Integrated Security=True;TrustServerCertificate=True;"));
            services.AddScoped<MyfinancesContext>();
            return services;
        }
    }
}
