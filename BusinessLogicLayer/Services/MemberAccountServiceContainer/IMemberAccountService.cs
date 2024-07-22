

using AllinOne.DataHandlers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.MemberAccountsServiceContainer
{
    public interface IMemberAccountService
    {
        Task<OutputHandler> Create(MemberAccountDTO memberAccount);
        Task<OutputHandler> Update(MemberAccountDTO memberAccount);
        Task<OutputHandler> Delete(int memberAccountId);
        Task<IEnumerable<MemberAccountDTO>> GetAllMemberAccounts();
        Task<MemberAccountDTO> GetMemberAccount(int memberAccountId);
    }
}

