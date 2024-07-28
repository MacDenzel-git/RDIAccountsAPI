using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.LoanConfigurationServiceContainer
{
    public class LoanConfigurationService : ILoanConfigurationService
    {
 
        private readonly GenericRepository<LoanConfiguration> _service;
        public LoanConfigurationService(GenericRepository<LoanConfiguration> service)
        {
             _service = service;
        }
        public async Task<OutputHandler> Create(LoanConfigurationDTO loanConfiguration)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.LoanConfigurationSpecifications == class.LoanConfigurationDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.LoanConfigurationDescription)

                //    };
                //}

                var mapped = new AutoMapper<LoanConfigurationDTO, LoanConfiguration>().MapToObject(loanConfiguration);
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
        public async Task<OutputHandler> Delete(int loanConfigurationId)
        {

            try
            {
                await _service.Delete(x => x.LoanConfigurationId == loanConfigurationId);
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

        public async Task<LoanConfigurationDTO> GetLoanConfiguration(int loanConfigurationId)
        {
            var output = await _service.GetSingleItem(x => x.LoanConfigurationId == loanConfigurationId);
            return new AutoMapper<LoanConfiguration, LoanConfigurationDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<LoanConfigurationDTO>> GetAllLoanConfigurations()
        {
            var output = await _service.GetAll();
            return new AutoMapper<LoanConfiguration, LoanConfigurationDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(LoanConfigurationDTO loanConfiguration)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.LoanDescription == loanConfiguration.LoanDescription);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(loanConfiguration.LoanDescription)

                    };
                }
                var mapped = new AutoMapper<LoanConfigurationDTO, LoanConfiguration>().MapToObject(loanConfiguration);
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
