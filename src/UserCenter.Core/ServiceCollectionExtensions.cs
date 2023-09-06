﻿using Microsoft.Extensions.DependencyInjection;
using UserCenter.Core.Abstracts;
using Yangtao.Hosting.Repository.MySql;

namespace UserCenter.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserCenterCore(this IServiceCollection services)
        {
            services.AddRepository();

            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IRoleProvider, RoleProvider>();

            return services;
        }
    }
}
