using LoanValidation.Services.Config;
using LoanValidation.Services.Interfaces;
using LoanValidation.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddScoped<ILogger, FileLogger>(provider =>
            {
                var config = provider.GetService<IAppConfig>();
                var filePath = config.LogFilePath; 
                return new FileLogger(filePath);
            });
            services.AddScoped<IErrorHandlingService, ErrorHandlingService>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
