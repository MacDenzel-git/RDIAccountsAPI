

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.TransactionTypesServiceContainer
{
    public interface ITransactionTypeService
    {
        Task<OutputHandler> Create(TransactionTypeDTO transactionType);
        Task<OutputHandler> Update(TransactionTypeDTO transactionType);
        Task<OutputHandler> Delete(int transactionTypeId);
        Task<IEnumerable<TransactionTypeDTO>> GetAllTransactionTypes();
        Task<TransactionTypeDTO> GetTransactionType(int transactionTypeId);
    }
}

