using System;

namespace LoanValidation.Services.Interfaces
{
    public interface ILogger
    {
        public void Log(string message);
        public void LogError(string message, Exception ex);
    }
}
