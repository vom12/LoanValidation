using LoanValidation.Services.Interfaces;
using System;

namespace LoanValidation.Services.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly ILogger _logger;

        public ErrorHandlingService(ILogger logger)
        {
            _logger = logger;
        }

        public void LogError(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }
}
