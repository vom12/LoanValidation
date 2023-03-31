using System.Collections.Generic;

namespace LoanValidation.Domain.Models
{
    public class ResultResponse
    {
        public string Decision { get; set; }
        public List<ValidationResult> ValidationResult { get; set; }
    }
}
