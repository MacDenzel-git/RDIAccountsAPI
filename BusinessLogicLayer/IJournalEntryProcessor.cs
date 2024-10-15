using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;

namespace BusinessLogicLayer
{
    public interface IJournalEntryProcessor
    {
        Task<OutputHandler> LoanDisburmentEntries(JournalEntryDTO entryDetails);
        Task<OutputHandler> LoanRepaymentEntries(JournalEntryDTO entryDetails);
        Task<OutputHandler> MemberShareJournalEntries(JournalEntryDTO entryDetails);
    }
}