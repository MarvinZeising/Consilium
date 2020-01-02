using System.Linq.Expressions;
using System.Linq;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Repository;
using Microsoft.IdentityModel.Logging;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["MySqlConnection:ConnectionString"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));

            services.AddAutoMapper(typeof(Startup));

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:Secret"]));
            services.AddAuthentication(x =>
            {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = "https://consiliumapp.org",
                    ValidateIssuer = true,
                    ValidIssuer = "https://consiliumapp.org",
                };
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userId = context.Principal.FindFirstValue(ClaimTypes.Sid);
                        var email = context.Principal.FindFirstValue(ClaimTypes.Email);
                        var db = context.HttpContext.RequestServices.GetRequiredService<IRepositoryWrapper>();
                        var user = db.User.GetById(new Guid(userId));
                        if (user == null || user.Email != email)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:8080")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.Configure<IISOptions>(options => { });

            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
        }
    }
}
