using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWithVueJs.Business.Database;
using CoreWithVueJs.Business.Factories;
using CoreWithVueJs.Business.Factories.Interfaces;
using CoreWithVueJs.Models.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreWithVueJs
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
            services.AddDbContext<CoreDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(setup =>
            {
                setup.Password.RequireDigit = true;
                setup.Password.RequiredLength = 8;
                setup.Password.RequireLowercase = true;
                setup.Password.RequireNonAlphanumeric = true;
                setup.Password.RequireUppercase = true;

                setup.User.AllowedUserNameCharacters += "������";
                setup.User.RequireUniqueEmail = true;
            });

            // Work on this later.
            services.AddAuthentication().AddCookie(options =>
            {
                options.Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                    SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest,
                    HttpOnly = true,
                    Name = "Authentication_cookie"
                };
            });

            services.AddScoped<IDataFactory, DataFactory>();

            services.AddDistributedMemoryCache();

            services.AddControllers();
            services.AddSpaStaticFiles(options => options.RootPath = "client-app/dist");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client-app";
                if (env.IsDevelopment())
                {
                    // Launch development server for Vue.js
                    spa.UseVueDevelopmentServer();
                }
            });
        }
    }
}