using System;
using System.Text;
using CoreWithVueJs.Business.Database;
using CoreWithVueJs.Business.Factories;
using CoreWithVueJs.Business.Factories.Interfaces;
using CoreWithVueJs.Models.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CoreWithVueJs
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(config => config.IdleTimeout = TimeSpan.FromHours(1));

            services.AddDbContext<CoreDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpContextAccessor();

            services.AddIdentity<ApplicationUser, IdentityRole>(setup =>
            {
                setup.Password.RequireDigit = true;
                setup.Password.RequiredLength = 8;
                setup.Password.RequireLowercase = true;
                setup.Password.RequireNonAlphanumeric = true;
                setup.Password.RequireUppercase = true;

                setup.User.AllowedUserNameCharacters += "åäöÅÄÖ";
                setup.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(token =>
                {
                    token.RequireHttpsMetadata = !Environment.IsDevelopment();
                    token.SaveToken = true;
                    token.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
                        ValidateIssuer = true,
                        ValidIssuer = "https://localhost:54051",
                        ValidateAudience = true,
                        ValidAudience = "https://localhost:54051",
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddCookie(options =>
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

            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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

            app.Use(async (_, next) => await next.Invoke().ConfigureAwait(false));

            app.UseSession();
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
