using LoanValidation.Services.Interfaces;
using System.Threading.Tasks;

namespace LoanValidation.Services.Services
{
    public class BusinessNumberService : IBusinessNumberService
    {

        public async Task<bool> ValidateBusinessNumber(string businessNumber)
        {
            if(!string.IsNullOrEmpty(businessNumber))
            {
                // simulate external call
                await Task.Delay(1000);

                return true;
            }

            return false;
        }
    }
}
