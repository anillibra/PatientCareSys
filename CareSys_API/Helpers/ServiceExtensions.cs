using CareSys_API.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            // Register Db Repository
            services.AddScoped<ICareHomeRepository, CareHomeRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
        }
    }
}
