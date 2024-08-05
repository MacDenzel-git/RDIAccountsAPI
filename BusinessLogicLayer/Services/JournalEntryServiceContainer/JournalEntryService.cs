using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using BusinessLogicLayer.JournalEntrysServiceContainer;
 using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.JournalEntryServiceContainer
{
    public class JournalEntryService : IJournalEntryService
    {

        private UnityOfWork _unitOfWork = new UnityOfWork();

        private readonly GenericRepository<JournalEntry> _service;
        private ILogger<JournalEntryService> _logger;
        public JournalEntryService(GenericRepository<JournalEntry> service, ILogger<JournalEntryService> logger)
        {
            _service = service;
            _logger = logger;
        }
        public async Task<OutputHandler> Create(JournalEntryDTO journalEntry)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.JournalEntrySpecifications == class.JournalEntryDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.JournalEntryDescription)

                //    };
                //}

                var mapped = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(journalEntry);
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
        public async Task<OutputHandler> Delete(int journalEntryId)
        {

            try
            {
                await _service.Delete(x => x.JournalEntryTransId == journalEntryId);
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

        public async Task<JournalEntryDTO> GetJournalEntry(int journalEntryId)
        {
            var output = await _service.GetSingleItem(x => x.JournalEntryTransId == journalEntryId);
            return new AutoMapper<JournalEntry, JournalEntryDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<JournalEntryDTO>> GetAllJournalEntrys()
        {
            var output = await _service.GetAll();
            return new AutoMapper<JournalEntry, JournalEntryDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(JournalEntryDTO journalEntry)
        {
            try
            {
                //  check record already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.JournalEntryDescription == journalEntry.JournalEntryDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(journalEntry.JournalEntryDescription)

                //    };
                //}
                var mapped = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(journalEntry);
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


        public async Task<OutputHandler> ShareTransaction(JournalEntryDTO   entryDetails)
        {
            //Who is making the share 
            try
            {
 
                _logger.LogInformation("Starting Share transaction entry");

                

                

                _unitOfWork.BeginTransaction();

                    
                  
                  var mappedEntryDetails = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(entryDetails);
                //C
                _logger.LogInformation("attempting Double entry: C");

                mappedEntryDetails.DrCr = "C";
                var outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogInformation($"Double entry Failed on C {outputHandler.Message}");
                    _unitOfWork.RollbackTransaction();
                    return outputHandler;
                }
                _logger.LogInformation("Double entry C entered");

                //D 
                mappedEntryDetails.DrCr = "D";
                mappedEntryDetails.JournalEntryTransId = 0;
                outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogInformation($"Double entry Failed on D {outputHandler.Message}");
                    _unitOfWork.RollbackTransaction();
                    return outputHandler;
                }

                _logger.LogInformation("Double entry D entered");
                _logger.LogInformation($"Updated Shares Made");
                //
                _logger.LogInformation($"Attempting to update Member Account with shares");

              var memberAccount = await  _unitOfWork.MemberAccountRepository.GetSingleItem(x => x.MemberAccountNumber == entryDetails.FromAccountNumber);

                memberAccount.TotalSharesContributed += entryDetails.AmountPaid;
                outputHandler =  await  _unitOfWork.MemberAccountRepository.Update(memberAccount);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogInformation($"Failed to update member Account {outputHandler.Message}");
                    _unitOfWork.RollbackTransaction();
                    return outputHandler;
                }
                _logger.LogInformation($"Updating Shares Contributed in Member Account - COMPLETED");

                _logger.LogInformation($"Updating Group Account Balance - DONE");
                var groupAccount = await _unitOfWork.GroupAccountRepository.GetSingleItem(x => x.GroupAccountNumber == entryDetails.ToAccountNumber);

                groupAccount.TotalSharesReceived += entryDetails.AmountPaid;
                outputHandler = await _unitOfWork.GroupAccountRepository.Update(groupAccount);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogInformation($"Failed to update Group Account {outputHandler.Message}");
                    _unitOfWork.RollbackTransaction();
                    return outputHandler;
                }

                _logger.LogInformation($"Updating Group Account Balance - COMPLETED");




                _unitOfWork.CommitTransaction();
                return new OutputHandler
                {

                };
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                _logger.LogError($"Rolling back ERROR: {ex}");

                return new OutputHandler { IsErrorOccured = true, Message =ex.Message };

            }

        }


        public async Task<OutputHandler> ShareTransactionApproval(JournalEntryDTO entryDetails)
        {
            //Who is making the share 
            var memberAccount = await _unitOfWork.MemberRepository.GetSingleItem(x => x.MemberId == entryDetails.MemberId);

        
              return new OutputHandler
            {

            };

        }

    }
}
