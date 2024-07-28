

using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.MemberDetailsServiceContainer
{
    public interface IMemberDetailService
    {
        Task<OutputHandler> Create(MemberDetailDTO memberDetail);
        Task<OutputHandler> Update(MemberDetailDTO memberDetail);
        Task<OutputHandler> Delete(int memberDetailId);
        Task<IEnumerable<MemberDetailDTO>> GetAllMemberDetails();
        Task<MemberDetailDTO> GetMemberDetail(int memberDetailId);
    }
}

