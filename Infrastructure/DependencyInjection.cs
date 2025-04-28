using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration configuration) 
        {
            Services.AddDbContext<ApplicationDbContext>(options =>  options.UseMySql(
                                                                configuration.GetConnectionString("DefaultConnection"),
                                                                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));
            return Services;
        }
    }
}
