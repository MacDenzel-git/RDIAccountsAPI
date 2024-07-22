using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using Blazored.SessionStorage;
using BusinessLogicLayer.Services.InterestAccountsServiceContainer;
using BusinessLogicLayer.Services.LoanConfigurationServiceContainer;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.ServiceContainer.InterestAccountServiceContainer
{
    public class InterestAccountService : IInterestAccountService
    {
        private readonly ISessionStorageService _sessionStorage;

        private readonly GenericRepository<InterestAccount> _service;
        public InterestAccountService(GenericRepository<InterestAccount> service, ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _service = service;
        }
        public async Task<OutputHandler> Create(InterestAccountDTO interestAccount)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.InterestAccountSpecifications == class.InterestAccountDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.InterestAccountDescription)

                //    };
                //}

                var mapped = new AutoMapper<InterestAccountDTO, InterestAccount>().MapToObject(interestAccount);
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
        public async Task<OutputHandler> Delete(int interestAccountId)
        {

            try
            {
                await _service.Delete(x => x.InterestAccountId == interestAccountId);
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

        public async Task<InterestAccountDTO> GetInterestAccount(int interestAccountId)
        {
            var output = await _service.GetSingleItem(x => x.InterestAccountId == interestAccountId);
            return new AutoMapper<InterestAccount, InterestAccountDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<InterestAccountDTO>> GetAllInterestAccounts()
        {
            var output = await _service.GetAll();
            return new AutoMapper<InterestAccount, InterestAccountDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(InterestAccountDTO interestAccount)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.InterestTitle == interestAccount.InterestTitle);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(interestAccount.InterestTitle)

                    };
                }
                var mapped = new AutoMapper<InterestAccountDTO, InterestAccount>().MapToObject(interestAccount);
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
