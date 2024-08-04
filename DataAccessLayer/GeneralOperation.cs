using AllinOne.DataHandlers.ErrorHandler;
using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class GeneralOperation
    {
        public static async Task<OutputHandler> GetAccountNumber(MemberDetailDTO memberDetail, string type)
        {
            //M093NTH01DM
            //Format M=Member/ G=Group, RandomNumber-GroupInitials, 01, Member Initials 

            try
            {
                string accountNumber = "";
                _logger.LogInformation("Attempting to Get Group Details to Create Account Number");

                var group = await _groupService.GetGroupDetail(memberDetail.GroupId);
                if (type.ToLower() == "member")
                {
                    _logger.LogInformation("member account creation started");

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

                    _logger.LogInformation($"Account Created {accountNumber}");
                    return new OutputHandler { IsErrorOccured = false, Message = "Success", Result = accountNumber };
                }
                else
                {
                    _logger.LogInformation("Group account creation started");

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
                    _logger.LogInformation($"Account Created {accountNumber}");

                    return new OutputHandler { IsErrorOccured = false, Message = "Success", Result = accountNumber };

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StandardMessages.getExceptionMessage(ex);
            }
            //M093NTH01DM
        }

    }
}
