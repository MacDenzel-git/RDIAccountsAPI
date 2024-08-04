using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using BusinessLogicLayer.Services.MemberAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.MemberAccountServiceContainer
{
    public class MemberAccountService : IMemberAccountService
    {
 
        private readonly GenericRepository<MemberAccount> _service;
        public MemberAccountService(GenericRepository<MemberAccount> service )
        {
             _service = service;
        }
        public async Task<OutputHandler> Create(MemberAccountDTO memberAccount)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.MemberAccountSpecifications == class.MemberAccountDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.MemberAccountDescription)

                //    };
                //}

                var mapped = new AutoMapper<MemberAccountDTO, MemberAccount>().MapToObject(memberAccount);
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
        public async Task<OutputHandler> Delete(int memberAccountId)
        {

            try
            {
                await _service.Delete(x => x.MemberAccountId == memberAccountId);
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

        public async Task<MemberAccountDTO> GetMemberAccount(int memberAccountId)
        {
            var output = await _service.GetSingleItem(x => x.MemberAccountId == memberAccountId);
            return new AutoMapper<MemberAccount, MemberAccountDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<MemberAccountDTO>> GetAllMemberAccounts()
        {
            var output = await _service.GetAll();
            return new AutoMapper<MemberAccount, MemberAccountDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(MemberAccountDTO memberAccount)
        {
            try
            {
                //  check record already exist to avoid duplicates
                // bool isExist = await _service.AnyAsync(x => x.AccountName == memberAccount.AccountName);
                // if (isExist)
                // {
                //     return new OutputHandler
                //     {
                //         IsErrorOccured = true,
                //         Message = StandardMessages.GetDuplicateMessage(memberAccount.AccountName)

                //     };
                // }
                var mapped = new AutoMapper<MemberAccountDTO, MemberAccount>().MapToObject(memberAccount);
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
