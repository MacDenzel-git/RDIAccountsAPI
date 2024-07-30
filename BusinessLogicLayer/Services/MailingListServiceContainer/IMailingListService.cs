

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.MailingListServiceContainer
{
    public interface IMailingListService
    {
        Task<OutputHandler> Create(MailingListDTO mailingList);
        Task<OutputHandler> Update(MailingListDTO mailingList);
        Task<OutputHandler> Delete(int mailingListId);
        Task<IEnumerable<MailingListDTO>> GetAllMailingLists();
        Task<MailingListDTO> GetMailingList(int mailingListId);
    }
}

