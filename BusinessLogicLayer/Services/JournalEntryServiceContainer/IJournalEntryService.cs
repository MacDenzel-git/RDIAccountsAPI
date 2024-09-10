

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.JournalEntrysServiceContainer
{
    public interface IJournalEntryService
    {
        Task<OutputHandler> Create(JournalEntryDTO journalEntry);
        Task<OutputHandler> ShareTransaction(JournalEntryDTO journalEntry);
        Task<OutputHandler> ShareTransactionSubmissionByMember(JournalEntryDTO journalEntry);
        Task<OutputHandler> LoanRequest(JournalEntryDTO journalEntry);
        Task<OutputHandler> LoanApproval(JournalEntryDTO journalEntry);
        Task<OutputHandler> LoanRepayments(JournalEntryDTO journalEntry);

        Task<OutputHandler> Update(JournalEntryDTO journalEntry);
        Task<OutputHandler> Delete(int journalEntryId);
        Task<IEnumerable<JournalEntryDTO>> GetAllJournalEntrys();
        Task<JournalEntryDTO> GetJournalEntry(int journalEntryId);
    }
}

