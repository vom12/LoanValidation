using System.Threading.Tasks;

namespace LoanValidation.Services.Interfaces
{
    public interface IBusinessNumberService
    {
        public Task<bool> ValidateBusinessNumber(string businessNumber);
    }
}
