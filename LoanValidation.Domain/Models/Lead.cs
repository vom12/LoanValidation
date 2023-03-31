namespace LoanValidation.Domain.Models
{
    public class Lead
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessNumber { get; set; }
        public decimal LoanAmount { get; set; }
        public string CitizenshipStatus { get; set; }
        public int TimeTrading { get; set; }
        public string CountryCode { get; set; }
        public string Industry { get; set; }
    }
}
