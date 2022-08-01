using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegisterationForm.DAL.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.IoC
{
    public static class DbConfig
    {
        public static IServiceCollection AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RegisterationFormDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("RegisterationFormDbContext"))
                );

            return services;
        }
    }
}
