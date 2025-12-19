using GMG.Application.Common.Interfaces;
using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Infrastructure.Persistence;
using GMG.Infrastructure.Persistence.Repositories;
using GMG.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBranchUserRepository, BranchUserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            return services;
        }
    }
}
