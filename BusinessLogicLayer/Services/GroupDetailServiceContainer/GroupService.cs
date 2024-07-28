using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
 using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.Services.LoanConfigurationServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.GroupDetailServiceContainer
{
    public class GroupDetailService : IGroupDetailService
    {

        private readonly GenericRepository<GroupDetail> _service;
        public GroupDetailService(GenericRepository<GroupDetail> service)
        {
            _service = service;
        }
        public async Task<OutputHandler> Create(GroupDetailDTO groupDetail)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates
                //bool isExist = await _service.AnyAsync(x => x.GroupDetailSpecifications == class.GroupDetailDescription);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(class.GroupDetailDescription)

                //    };
                //}

                var mapped = new AutoMapper<GroupDetailDTO, GroupDetail>().MapToObject(groupDetail);
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
        public async Task<OutputHandler> Delete(int groupDetailId)
        {

            try
            {
                await _service.Delete(x => x.GroupId == groupDetailId);
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

        public async Task<GroupDetailDTO> GetGroupDetail(int groupDetailId)
        {
            var output = await _service.GetSingleItem(x => x.GroupId == groupDetailId);
            return new AutoMapper<GroupDetail, GroupDetailDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<GroupDetailDTO>> GetAllGroupDetails()
        {
            var output = await _service.GetAll();
            return new AutoMapper<GroupDetail, GroupDetailDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(GroupDetailDTO groupDetail)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.GroupName == groupDetail.GroupName);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(groupDetail.GroupName)

                    };
                }
                var mapped = new AutoMapper<GroupDetailDTO, GroupDetail>().MapToObject(groupDetail);
                //mapped.ModifiedDate = DateTime.Now;
                //mapped.ModifiedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");

                var result = await _service.Update(mapped);
                return result;

            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }

    }
}
