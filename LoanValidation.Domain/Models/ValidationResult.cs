namespace LoanValidation.Domain.Models
{
    public class ValidationResult
    {
        public string Rule { get; set; }
        public string Message { get; set; }
        public string Decision { get; set; }
    }
}
