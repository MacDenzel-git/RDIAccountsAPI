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
                var memberAccount = await _unitOfWork.MemberRepository.GetSingleItem(x => x.MemberId == entryDetails.MemberId);

                _logger.LogInformation("Starting Share transaction entry");

                

                

                _unitOfWork.BeginTransaction();

                var mappedEntryDetails = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(entryDetails);
                var journalEntryResult = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);

                _logger.LogInformation($"Updated Shares Made");
                _logger.LogInformation($"Getting Associated Member Account");

              var memberDetails = await  _unitOfWork.MemberRepository.GetSingleItem(x => x.MemberId == entryDetails.MemberId);
                _logger.LogInformation($"Updating Shares Contributed in Member Account");
              //memberDetails.ba

                _logger.LogInformation($"Updating Main Account Balance");




                _unitOfWork.CommitTransaction();
                return new OutputHandler
                {

                };
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
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
