using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using Blazored.SessionStorage;
using BusinessLogicLayer.Services.TransactionTypesServiceContainer;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.ServiceContainer.TransactionTypeServiceContainer
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ISessionStorageService _sessionStorage;

        private readonly GenericRepository<TransactionType> _service;
        public TransactionTypeService(GenericRepository<TransactionType> service, ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _service = service;
        }
        public async Task<OutputHandler> Create(TransactionTypeDTO transactionType)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.TransactionTypeSpecifications == class.TransactionTypeDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.TransactionTypeDescription)

                //    };
                //}

                var mapped = new AutoMapper<TransactionTypeDTO, TransactionType>().MapToObject(transactionType);
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
        public async Task<OutputHandler> Delete(int transactionTypeId)
        {

            try
            {
                await _service.Delete(x => x.TransactionTypeId == transactionTypeId);
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

        public async Task<TransactionTypeDTO> GetTransactionType(int transactionTypeId)
        {
            var output = await _service.GetSingleItem(x => x.TransactionTypeId == transactionTypeId);
            return new AutoMapper<TransactionType, TransactionTypeDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<TransactionTypeDTO>> GetAllTransactionTypes()
        {
            var output = await _service.GetAll();
            return new AutoMapper<TransactionType, TransactionTypeDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(TransactionTypeDTO transactionType)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.TransactionTypeDescription == transactionType.TransactionTypeDescription);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(transactionType.TransactionTypeDescription)

                    };
                }
                var mapped = new AutoMapper<TransactionTypeDTO, TransactionType>().MapToObject(transactionType);
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
