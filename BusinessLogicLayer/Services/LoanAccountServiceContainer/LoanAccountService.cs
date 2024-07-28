using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using BusinessLogicLayer.Services.LoanAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.LoanAccountServiceContainer
{
    public class LoanAccountService : ILoanAccountService
    {
 
        private readonly GenericRepository<LoanAccount> _service;
        public LoanAccountService(GenericRepository<LoanAccount> service)
        {
             _service = service;
        }
        public async Task<OutputHandler> Create(LoanAccountDTO loanAccount)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.LoanAccountSpecifications == class.LoanAccountDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.LoanAccountDescription)

                //    };
                //}

                var mapped = new AutoMapper<LoanAccountDTO, LoanAccount>().MapToObject(loanAccount);
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
        public async Task<OutputHandler> Delete(int loanAccountId)
        {

            try
            {
                await _service.Delete(x => x.Id == loanAccountId);
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

        public async Task<LoanAccountDTO> GetLoanAccount(int loanAccountId)
        {
            var output = await _service.GetSingleItem(x => x.Id == loanAccountId);
            return new AutoMapper<LoanAccount, LoanAccountDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<LoanAccountDTO>> GetAllLoanAccounts()
        {
            var output = await _service.GetAll();
            return new AutoMapper<LoanAccount, LoanAccountDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(LoanAccountDTO loanAccount)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.MemberAccountNumber == loanAccount.MemberAccountNumber);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(loanAccount.MemberAccountNumber)

                    };
                }
                var mapped = new AutoMapper<LoanAccountDTO, LoanAccount>().MapToObject(loanAccount);
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
