

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.GroupDetailsServiceContainer
{
    public interface IGroupDetailService
    {
        Task<OutputHandler> Create(GroupDetailDTO groupDetail);
        Task<OutputHandler> Createv1(GroupDetailDTO groupDetail);
        Task<OutputHandler> Update(GroupDetailDTO groupDetail);
        Task<OutputHandler> Delete(int groupDetailId);
        Task<IEnumerable<GroupDetailDTO>> GetAllGroupDetails();
        Task<GroupDetailDTO> GetGroupDetail(int groupDetailId);
    }
}

