using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareSys_API.Models;
using CareSys_API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using CareSys_API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;



namespace CareSys.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // ===== Add our DbContext ========
            // Add DB context and configure
            services.AddDbContext<CareSysContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CareSys"));
            });

            // ===== Add Identity ========
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CareSysContext>()
                .AddDefaultTokenProviders();

            //services.AddAuthorization(auth =>
            //{
            //    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            //        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
            //        .RequireAuthenticatedUser().Build());
            //});

            // Define Policy
            var policyForCreate = new AuthorizationPolicyBuilder()
              .AddAuthenticationSchemes("Bearer")
              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
              .RequireAuthenticatedUser()
              // .RequireRole("Admin")
              .RequireClaim("CanCreateCareHome", "True")
              .Build();

            var policyForRead = new AuthorizationPolicyBuilder()
             .AddAuthenticationSchemes("Bearer")
             .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
             .RequireAuthenticatedUser()
             // .RequireRole("Admin")
             .RequireClaim("CanReadCareHome", "True")
             .Build();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanCreateCareHome", policyForCreate);
                options.AddPolicy("CanReadCareHome", policyForRead);
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanCreateCareHome", p => p.RequireClaim("CanCreateCareHome"));
            //    options.AddPolicy("CanReadCareHome", p => p.RequireClaim("CanReadCareHome"));
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CareSys_API_Super_Key")),
                    ValidAudience = "http://localhost:4200",
                    ValidIssuer = "http://localhost:51004",
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });

            // Register Db Repository
            // services.AddScoped<ICareHomeRepository, CareHomeRepository>();
            services.ConfigureRepositories();

            services.AddAutoMapper();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanCreateCareHome", p => p.RequireClaim("CanCreateCareHome"));
            });

            // services.AddMvc();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler(); // This will Catch all unhanled exception at Application, No need to write try catch in each action 
                // Now let's add 500 error with same error message
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        // Throw 500 error
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        // Send error message code to response
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }


            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();

            SeedUsers(serviceProvider).Wait();
            app.UseMvcWithDefaultRoute(); // This enables Attribute Routing

        }

        private async Task SeedUsers(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            string[] roleNames = { "Admin", "Manager", "Member", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new AppUser
            {
                UserName = "SuperUser",
                Email = "super@some.com",
            };
            //Ensure you have these values in your appsettings.json file
            // Create SuperUser with Admin Role and with CanCreateCareHome Claim
            string userPWD = "Pass@123";
            var _user = await UserManager.FindByEmailAsync("super@some.com");
            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {                   
                    await UserManager.AddToRoleAsync(poweruser, "Admin");                   
                }
            }
            // Add claim
            await UserManager.RemoveClaimAsync(_user, new Claim("CanCreateCareHome", "True"));
            await UserManager.RemoveClaimAsync(_user, new Claim("CanReadCareHome", "True"));

            var claimResult = await UserManager.AddClaimAsync(_user, new Claim("CanCreateCareHome", "True"));
            var claimResult2 = await UserManager.AddClaimAsync(_user, new Claim("CanReadCareHome", "True"));

            ////=============================
            //// User Bob, Role Member, Claim - CanReadCareHome
            var bob = new AppUser
            {
                UserName = "Bob",
                Email = "bob@some.com",
            };

            var _bob = await UserManager.FindByEmailAsync("bob@some.com");
            if (_bob == null)
            {
                var createBobUser = await UserManager.CreateAsync(bob, userPWD);
                if (createBobUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(bob, "User");
                }
            }
            // Add claim
            await UserManager.RemoveClaimAsync(_bob, new Claim("CanReadCareHome", "True"));
            var claimResult21 = await UserManager.AddClaimAsync(_bob, new Claim("CanReadCareHome", "True"));

            ////=============================
            //// User Martyn, Role Member, Claim - No
            var martyn = new AppUser
            {
                UserName = "Martyn",
                Email = "martyn@some.com",
            };

            var _martyn = await UserManager.FindByEmailAsync("martyn@some.com");
            if (_martyn == null)
            {
                var createMartynUser = await UserManager.CreateAsync(martyn, userPWD);
                if (createMartynUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(martyn, "User");
                }
            }
        }
    }
}
