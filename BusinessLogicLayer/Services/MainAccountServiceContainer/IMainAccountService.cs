

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.MainAccountsServiceContainer
{
    public interface IMainAccountService
    {
        Task<OutputHandler> Create(MainAccountDTO mainAccount);
        Task<OutputHandler> Update(MainAccountDTO mainAccount);
        Task<OutputHandler> Delete(int mainAccountId);
        Task<IEnumerable<MainAccountDTO>> GetAllMainAccounts();
        Task<MainAccountDTO> GetMainAccount(int mainAccountId);
    }
}

