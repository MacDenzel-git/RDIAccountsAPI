using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using BusinessLogicLayer.Services.GroupAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.GroupAccountServiceContainer
{
    public class GroupAccountService : IGroupAccountService
    {
 
        private readonly GenericRepository<GroupAccount> _service;
        public GroupAccountService(GenericRepository<GroupAccount> service )
        {
             _service = service;
        }
        public async Task<OutputHandler> Create(GroupAccountDTO groupAccount)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.GroupAccountSpecifications == class.GroupAccountDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.GroupAccountDescription)

                //    };
                //}

                var mapped = new AutoMapper<GroupAccountDTO, GroupAccount>().MapToObject(groupAccount);
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
        public async Task<OutputHandler> Delete(int groupAccountId)
        {

            try
            {
                await _service.Delete(x => x.GroupAccountId == groupAccountId);
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

        public async Task<GroupAccountDTO> GetGroupAccount(int groupAccountId)
        {
            var output = await _service.GetSingleItem(x => x.GroupAccountId == groupAccountId);
            return new AutoMapper<GroupAccount, GroupAccountDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<GroupAccountDTO>> GetAllGroupAccounts()
        {
            var output = await _service.GetAll();
            return new AutoMapper<GroupAccount, GroupAccountDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(GroupAccountDTO groupAccount)
        {
            try
            {
                //  check record already exist to avoid duplicates
                // bool isExist = await _service.AnyAsync(x => x.AccountName == groupAccount.AccountName);
                // if (isExist)
                // {
                //     return new OutputHandler
                //     {
                //         IsErrorOccured = true,
                //         Message = StandardMessages.GetDuplicateMessage(groupAccount.AccountName)

                //     };
                // }
                var mapped = new AutoMapper<GroupAccountDTO, GroupAccount>().MapToObject(groupAccount);
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
