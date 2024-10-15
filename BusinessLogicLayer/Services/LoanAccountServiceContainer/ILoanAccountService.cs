

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.LoanAccountsServiceContainer
{
    public interface ILoanAccountService
    {
        Task<OutputHandler> CreateLoan(LoanAccountDTO loanAccount);
        Task<OutputHandler> Update(LoanAccountDTO loanAccount);
        Task<OutputHandler> Delete(int loanAccountId);
        Task<IEnumerable<LoanAccountDTO>> GetAllLoanAccounts();
        Task<LoanAccountDTO> GetLoanAccount(int loanAccountId);

        Task<OutputHandler> ApproveLoan(RequestApprovalDTO loanAccountDTO);
    }
}

