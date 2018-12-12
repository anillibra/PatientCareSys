using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CareSys_API.Models;
using Microsoft.EntityFrameworkCore;
using CareSys_API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CareSys.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // BuildWebHost(args).Run();
            var host = BuildWebHost(args);

            // migrate & seed the database.  Best practice = in Main, using service scope
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    // get CareSys Context from registed services
                    var CareSysContext = scope.ServiceProvider.GetRequiredService<CareSysContext>();

                    // Get Hosting Environment
                    var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                    if (!hostingEnvironment.IsProduction())
                    {
                        CareSysContext.Database.Migrate();
                    }
                }
                catch(Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred with migrating or seeding the DB.");
                }
            }

            // run the web app
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
