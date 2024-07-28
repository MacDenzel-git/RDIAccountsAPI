using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using BusinessLogicLayer.JournalEntrysServiceContainer;
 using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.JournalEntryServiceContainer
{
    public class JournalEntryService : IJournalEntryService
    {
 
        private readonly GenericRepository<JournalEntry> _service;
        public JournalEntryService(GenericRepository<JournalEntry> service)
        {
             _service = service;
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

    }
}
