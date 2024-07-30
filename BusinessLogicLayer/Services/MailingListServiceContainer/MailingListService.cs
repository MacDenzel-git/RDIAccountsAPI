using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.MailingListServiceContainer
{
    public class MailingListService : IMailingListService
    {

        private readonly GenericRepository<MailingList> _service;
        public MailingListService(GenericRepository<MailingList> service)
        {
            _service = service;
        }
        public async Task<OutputHandler> Create(MailingListDTO mailingList)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.MailingListSpecifications == class.MailingListDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.MailingListDescription)

                //    };
                //}

                var mapped = new AutoMapper<MailingListDTO, MailingList>().MapToObject(mailingList);
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
        public async Task<OutputHandler> Delete(int mailingListId)
        {

            try
            {
                await _service.Delete(x => x.Id == mailingListId);
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

        public async Task<MailingListDTO> GetMailingList(int mailingListId)
        {
            var output = await _service.GetSingleItem(x => x.Id == mailingListId);
            return new AutoMapper<MailingList, MailingListDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<MailingListDTO>> GetAllMailingLists()
        {
            var output = await _service.GetAll();
            return new AutoMapper<MailingList, MailingListDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(MailingListDTO mailingList)
        {
            try
            {
                 
                var mapped = new AutoMapper<MailingListDTO, MailingList>().MapToObject(mailingList);
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
