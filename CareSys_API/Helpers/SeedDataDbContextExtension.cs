using CareSys_API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Helpers
{
    public static class SeedDataDbContextExtension
    {
        public static void EnsureSeedData(this CareSysContext context)
        {
            // On Dev environment, Clear the database, ensure fresh database each time we start Demo (App)
            context.CareHomes.RemoveRange(context.CareHomes);
            context.Users.RemoveRange(context.Users);

            context.SaveChanges();

            // Init Data
            var CareHomes = new List<CareHome>()
            {
                new CareHome()
                {
                    Name = "Prod1111 Old Age Care Home",
                    Address = "#, 123London",
                    Category = "Old Age",
                    IsActive = true,
                    Location = "London",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 10001
                },
                new CareHome()
                {
                    Name = "Mid Age Care Home",
                    Address = "#, 124 London",
                    Category = "Mid Age Group",
                    IsActive = false,
                    Location = "London",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 10001
                },
                new CareHome()
                {
                    Name = "Child Care Home",
                    Address = "#, 1001 London",
                    Category = "Child Age Group",
                    IsActive = false,
                    Location = "London",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 10002
                },
                new CareHome()
                {
                    Name = "New Born Care Home",
                    Address = "#, 1002 London",
                    Category = "Baby Child Age Group",
                    IsActive = true,
                    Location = "London",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 10004
                }
            };

            context.CareHomes.AddRange(CareHomes);
            context.SaveChanges();
        }
    }

   
}
