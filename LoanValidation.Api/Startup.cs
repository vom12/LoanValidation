using LoanValidation.Services.Config;
using LoanValidation.Services.Interfaces;
using LoanValidation.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LoanValidation.Api
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
            services.AddControllers();

            services.AddScoped<IAppConfig, AppConfig>();
            services.AddScoped<ILoanValidationService, LoanValidationService>();
            services.AddScoped<ILoanValidationCache, LoanValidationCache>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBusinessNumberService, BusinessNumberService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILogger, FileLogger>(provider =>
            {
                var config = provider.GetService<IAppConfig>();
                var filePath = config.LogFilePath; 
                return new FileLogger(filePath);
            });
            services.AddScoped<IErrorHandlingService, ErrorHandlingService>();

            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.GetValue<string>("Issuer"),

                ValidateAudience = true,
                ValidAudience = jwtSettings.GetValue<string>("Audience"),

                ValidateLifetime = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Secret"))),
                ValidateIssuerSigningKey = true,
            };
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
