using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.MemberDetailServiceContainer
{
    public class MemberDetailService : IMemberDetailService
    {

        private readonly GenericRepository<MemberDetails> _service;
        private readonly IMainAccountService _accountService;
        private readonly IGroupDetailService _groupService;

        public MemberDetailService(GenericRepository<MemberDetails> service, IMainAccountService accountService, IGroupDetailService groupService)
        {
            _service = service;
            _accountService = accountService;
            _groupService = groupService;
        }
        public async Task<OutputHandler> Create(MemberDetailDTO memberDetail)
        {
            try
            {
                //check if record with same name already exist to avoid duplicates  - check phone number
                bool isExist = await _service.AnyAsync(x => x.PhoneNumber == memberDetail.PhoneNumber);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

                    };
                }
                //Get Account nnumber
                //Compute Variables used to Create an account.
                string accountNumber = "";

                ///#######LOG ENTRY
                var output = await GetAccountNumber(memberDetail, "member");
                //########LOG EXIT 
                if (output.IsErrorOccured)
                {
                    //if there's an error go back
                    return new OutputHandler { IsErrorOccured = true, Message = output.Message };
                }
                else
                {
                    accountNumber = output.Result.ToString();
                    if (accountNumber is null)
                    {
                        return new OutputHandler
                        {
                            Message = "Failed to Compute Account Number, Result returned null",
                            IsErrorOccured = true,
                           
                        };
                    }
                }
                //Create Account 
                //LOG ENTRY CREATE ACCOUNT


                //LOG EXIT 
                var account = new MainAccountDTO
                {
                    AccountName = memberDetail.MemberName,
                    AccountType = "Member",
                    Balance = 0,
                    AccountNumber = accountNumber

                };

                var mapped = new AutoMapper<MemberDetailDTO, MemberDetails>().MapToObject(memberDetail);
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

        private async Task<OutputHandler> GetAccountNumber(MemberDetailDTO memberDetail, string type)
        {
            //M093NTH01DM
            //Format M=Member/ G=Group, RandomNumber-GroupInitials, 01, Member Initials 

            try
            {
                string accountNumber = "";
                var group = await _groupService.GetGroupDetail(memberDetail.GroupId);
                if (type.ToLower() == "member")
                {
                    string memberId = "";
                    if (memberDetail.MemberId < 10)
                    {
                        memberId = $"0{memberId}";
                    }
                    else
                    {
                        memberId = memberDetail.MemberId.ToString();
                    }
                    string initials = memberDetail.MemberName.GetInitials();
                    accountNumber = $"M0193{group.GroupInitials}G{memberDetail.GroupId}{memberId}{initials}";
                    return new OutputHandler { IsErrorOccured = false, Message = "Success", Result = accountNumber };
                }
                else
                {
                    string groupId = "";
                    if (memberDetail.GroupId < 10)
                    {
                        groupId = $"0{groupId}";
                    }
                    else
                    {
                        groupId = memberDetail.GroupId.ToString();
                    }


                    accountNumber = $"G01295{group.GroupInitials}{memberDetail.GroupId}";
                    return new OutputHandler { IsErrorOccured = false, Message = "Success", Result = accountNumber };

                }
            }
            catch (Exception ex)
            {

                return StandardMessages.getExceptionMessage(ex);
            }
            //M093NTH01DM
        }

        //Code for deleting record
        public async Task<OutputHandler> Delete(int memberDetailId)
        {

            try
            {
                await _service.Delete(x => x.MemberId == memberDetailId);
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

        public async Task<MemberDetailDTO> GetMemberDetail(int memberDetailId)
        {
            var output = await _service.GetSingleItem(x => x.MemberId == memberDetailId);
            return new AutoMapper<MemberDetails, MemberDetailDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<MemberDetailDTO>> GetAllMemberDetails()
        {
            var output = await _service.GetAll();
            return new AutoMapper<MemberDetails, MemberDetailDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(MemberDetailDTO memberDetail)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.MemberId == memberDetail.MemberId);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

                    };
                }
                var mapped = new AutoMapper<MemberDetailDTO, MemberDetails>().MapToObject(memberDetail);
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
