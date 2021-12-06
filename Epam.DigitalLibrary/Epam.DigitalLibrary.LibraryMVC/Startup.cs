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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SecureString password = new SecureString();

            password.AppendChar('1');
            password.AppendChar('2');
            password.AppendChar('3');

            password.MakeReadOnly();

            SqlCredential credential = new SqlCredential("lib_admin", password);

            services.AddSingleton<INoteLogic>(new LibraryLogic(Configuration.GetConnectionString("SSPIConnString"), credential));
            services.AddSingleton<IUserRightsProvider>(new UserLogic(Configuration.GetConnectionString("SSPIConnString"), credential));

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
            var a = Configuration.GetConnectionString("SSPIConnString");

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
