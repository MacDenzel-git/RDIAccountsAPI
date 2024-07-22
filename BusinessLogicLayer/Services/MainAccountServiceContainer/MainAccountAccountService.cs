using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using Blazored.SessionStorage;
using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.ServiceContainer.MainAccountServiceContainer
{
    public class MainAccountService : IMainAccountService
    {
        private readonly ISessionStorageService _sessionStorage;

        private readonly GenericRepository<MainAccount> _service;
        public MainAccountService(GenericRepository<MainAccount> service, ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _service = service;
        }
        public async Task<OutputHandler> Create(MainAccountDTO mainAccount)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.MainAccountSpecifications == class.MainAccountDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.MainAccountDescription)

                //    };
                //}

                var mapped = new AutoMapper<MainAccountDTO, MainAccount>().MapToObject(mainAccount);
                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                var result = await _service.Create(mapped);
                return result;
            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }

        }

        //Code for deleting record
        public async Task<OutputHandler> Delete(int mainAccountId)
        {

            try
            {
                await _service.Delete(x => x.AccountId == mainAccountId);
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = StandardMessages.GetSuccessfulMessage() //assign message to the error
                };
            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }
        }

        public async Task<MainAccountDTO> GetMainAccount(int mainAccountId)
        {
            var output = await _service.GetSingleItem(x => x.AccountId == mainAccountId);
            return new AutoMapper<MainAccount, MainAccountDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<MainAccountDTO>> GetAllMainAccounts()
        {
            var output = await _service.GetAll();
            return new AutoMapper<MainAccount, MainAccountDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(MainAccountDTO mainAccount)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.AccountName == mainAccount.AccountName);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(mainAccount.AccountName)

                    };
                }
                var mapped = new AutoMapper<MainAccountDTO, MainAccount>().MapToObject(mainAccount);
                //mapped.ModifiedDate = DateTime.Now;
                //mapped.ModifiedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");

                var result = await _service.Update(mapped);
                return result;

            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }
        }

    }
}
