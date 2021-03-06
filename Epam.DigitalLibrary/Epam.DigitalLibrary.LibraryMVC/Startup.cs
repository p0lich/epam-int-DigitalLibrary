using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.Logic;
using System.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Data.SqlClient;
using Epam.DigitalLibrary.LibraryMVC.CustomIdentity;
using Epam.DigitalLibrary.LibraryMVC.CustomEncryption;

namespace Epam.DigitalLibrary.LibraryMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INoteLogic>(new LibraryLogic(Configuration.GetConnectionString("SSPIConnString")));
            services.AddSingleton<IUserRightsProvider>(new UserLogic(Configuration.GetConnectionString("SSPIConnString")));

            services.AddTransient<IUserRoleProvider, CustomUserRoleProvider>();
            services.AddTransient<ISHA512HashCompute, SHA512Compute>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/Authenticate/LoginRedirect";
                    options.AccessDeniedPath = "/Authenticate/Denied";
                });

            services.AddControllersWithViews();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{secondId?}");
            });
        }
    }
}
