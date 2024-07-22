

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.InterestAccountsServiceContainer
{
    public interface IInterestAccountService
    {
        Task<OutputHandler> Create(InterestAccountDTO interestAccount);
        Task<OutputHandler> Update(InterestAccountDTO interestAccount);
        Task<OutputHandler> Delete(int interestAccountId);
        Task<IEnumerable<InterestAccountDTO>> GetAllInterestAccounts();
        Task<InterestAccountDTO> GetInterestAccount(int interestAccountId);
    }
}

