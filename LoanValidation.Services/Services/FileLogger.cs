using LoanValidation.Services.Interfaces;
using System;
using System.IO;

namespace LoanValidation.Services.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            try
            {
                using StreamWriter sw = File.AppendText(_logFilePath);

                sw.WriteLine($"[{DateTime.Now}] {message}");

                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        public void LogError(string message, Exception ex)
        {
            try
            {
                using StreamWriter sw = File.AppendText(_logFilePath);
                sw.WriteLine($"[{DateTime.Now}] ERROR: {message}");
                sw.WriteLine($"Exception message: {ex.Message}");
                sw.WriteLine($"Stack trace: {ex.StackTrace}");

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing to log file: {e.Message}");
            }
        }
    }
}
