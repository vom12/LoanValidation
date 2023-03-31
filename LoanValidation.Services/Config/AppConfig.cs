using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LoanValidation.Services.Config
{
    public class AppConfig : IAppConfig
    {
        private readonly IConfiguration _config;

        public AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _config = builder.Build();
        }

        public string AllowedCitizenshipStatuses => _config["AllowedCitizenshipStatuses"];
        public string AllowedCountryCodes => _config["AllowedCountryCodes"];
        public string AllowedIndustries => _config["AllowedIndustries"];
        public string BannedIndustries => _config["BannedIndustries"];
        public int MinLoanAmount => int.Parse(_config["MinLoanAmount"]);
        public int MaxLoanAmount => int.Parse(_config["MaxLoanAmount"]);
        public int MinTimeTrading => int.Parse(_config["MinTimeTrading"]);
        public int MaxTimeTrading => int.Parse(_config["MaxTimeTrading"]);
        public int CacheExpirationMinutes => int.Parse(_config["CacheExpirationMinutes"]);

        string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        string logFolder = "logs";
        public string LogFilePath => Path.Combine(rootPath, logFolder, "app.log");
        public string LogLevel => _config["Logging:LogLevel:Default"];
    }
}
