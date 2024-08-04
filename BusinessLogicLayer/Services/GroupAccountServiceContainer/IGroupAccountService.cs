

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.GroupAccountsServiceContainer
{
    public interface IGroupAccountService
    {
        Task<OutputHandler> Create(GroupAccountDTO groupAccount);
        Task<OutputHandler> Update(GroupAccountDTO groupAccount);
        Task<OutputHandler> Delete(int groupAccountId);
        Task<IEnumerable<GroupAccountDTO>> GetAllGroupAccounts();
        Task<GroupAccountDTO> GetGroupAccount(int groupAccountId);
    }
}

