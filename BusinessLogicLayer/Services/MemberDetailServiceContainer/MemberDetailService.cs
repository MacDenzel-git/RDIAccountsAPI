using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.Services.MailingListServiceContainer;
using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using BusinessLogicLayer.UnitOfWorkContainer;
using BusinessLogicLayer.UnitOfWorkContainer.ContosoUniversity.DAL;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RDIAccountsAPI;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BusinessLogicLayer.Services.MemberDetailServiceContainer
{
    public class MemberDetailService : IMemberDetailService
    {
        private readonly IMailingListService _mailingListService;
        private readonly GenericRepository<MemberDetail> _memberDetailService;
        private readonly IMainAccountService _mainAccountService;
        private readonly IGroupDetailService _groupService;
        private readonly EasyAccountDbContext _context;
        private ILogger<MemberDetailService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MemberDetailService(GenericRepository<MemberDetail> service, IMainAccountService accountService, IGroupDetailService groupService, ILogger<MemberDetailService> logger, IMailingListService mailingListService, EasyAccountDbContext context, IUnitOfWork unitOfWork)
        {
            _memberDetailService = service;
            _mainAccountService = accountService;
            _groupService = groupService;
            _logger = logger;
            _mailingListService = mailingListService;
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<OutputHandler> Create(MemberDetailDTO memberDetail)
        {

 
            try
            {
                //check if record with same name already exist to avoid duplicates  - check phone number
                bool isExist = await _memberDetailService.AnyAsync(x => x.PhoneNumber == memberDetail.PhoneNumber);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

                    };
                }

                _logger.LogInformation("Checked if Duplicate:No Duplicate");
                //Get Account nnumber
                //Compute Variables used to Create an account.
                string accountNumber = "";


                _logger.LogInformation("Started Member Creation");
                _logger.LogInformation("Create Account Number");

                var output = await GetAccountNumber(memberDetail, "member");


                if (output.IsErrorOccured)
                {
                    _logger.LogInformation($"Exited Account Creation with an Error {output.Message}");
                    //if there's an error go back
                    return new OutputHandler { IsErrorOccured = true, Message = output.Message };
                }
                else
                {
                    _logger.LogInformation($"Exited Account Successfully {output.Message}");

                    accountNumber = output.Result.ToString();
                    if (accountNumber is null)
                    {
                        _logger.LogInformation($"Exited Account Creation successfully but account Number is Empty returning Error");

                        return new OutputHandler
                        {
                            Message = "Failed to Compute Account Number, Result returned null",
                            IsErrorOccured = true,

                        };
                    }
                }
                //Create Account 
                //LOG ENTRY CREATE ACCOUNT
                _logger.LogInformation($"Creating an account for member");

                _unitOfWork.BeginTransaction();

                var account = new MainAccountDTO
                {
                    AccountName = memberDetail.MemberName,
                    AccountType = "Member",
                    Balance = 0,
                    AccountNumber = accountNumber

                };

                var mappedAccount = new AutoMapper<MainAccountDTO, MainAccount>().MapToObject(account);

                _logger.LogInformation($"Attempting to Create an Account Record in Main Account Table");
                _unitOfWork._iMainAccountService.Create(account);
                _logger.LogInformation($"Main Account Created");

                _logger.LogInformation($"Attempting to Add Email to Mailing List Table");

                //add email to Mailing List 
                var mailingListDetail = new MailingListDTO
                {
                    GroupId = memberDetail.GroupId,
                    Email = memberDetail.Email
                };
                _unitOfWork._iMailingListService.Create(mailingListDetail);
                _logger.LogInformation($"Email added to Mailing List Successfully");

                




                var mapped = new AutoMapper<MemberDetailDTO, MemberDetail>().MapToObject(memberDetail);
                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                _logger.LogInformation($"Attempting to add Member Details to Database");
                _unitOfWork._iMemberDetailService.Create(memberDetail);
                _logger.LogInformation($"Member Details Successfully Added");

                _logger.LogInformation("Attempting to Commit Transaction Scope");
                _unitOfWork.CommitTransaction();
                _logger.LogInformation($"Transaction Scope Competed for Member {memberDetail.MemberName}-{accountNumber}");

                return new OutputHandler { IsErrorOccured = false, Message = "Successfully Completed"};

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong, Attempting to Rollback ERROR:{ex}");

                _unitOfWork.RollbackTransaction();

                _logger.LogInformation($"Rollback Complete");

                return StandardMessages.getExceptionMessage(ex);
            }

        }


        //public async Task<OutputHandler> CreateWithUOW(MemberDetailDTO memberDetail)
        //{


        //    try
        //    {
        //        //check if record with same name already exist to avoid duplicates  - check phone number
        //        bool isExist = await _memberDetailService.AnyAsync(x => x.PhoneNumber == memberDetail.PhoneNumber);
        //        if (isExist)
        //        {
        //            return new OutputHandler
        //            {
        //                IsErrorOccured = true,
        //                Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

        //            };
        //        }

        //        _logger.LogInformation("Checked if Duplicate:No Duplicate");
        //        //Get Account nnumber
        //        //Compute Variables used to Create an account.
        //        string accountNumber = "";


        //        _logger.LogInformation("Started Member Creation");
        //        _logger.LogInformation("Create Account Number");

        //        var output = await GetAccountNumber(memberDetail, "member");


        //        if (output.IsErrorOccured)
        //        {
        //            _logger.LogInformation($"Exited Account Creation with an Error {output.Message}");
        //            //if there's an error go back
        //            return new OutputHandler { IsErrorOccured = true, Message = output.Message };
        //        }
        //        else
        //        {
        //            _logger.LogInformation($"Exited Account Successfully {output.Message}");

        //            accountNumber = output.Result.ToString();
        //            if (accountNumber is null)
        //            {
        //                _logger.LogInformation($"Exited Account Creation successfully but account Number is Empty returning Error");

        //                return new OutputHandler
        //                {
        //                    Message = "Failed to Compute Account Number, Result returned null",
        //                    IsErrorOccured = true,

        //                };
        //            }
        //        }
        //        //Create Account 
        //        //LOG ENTRY CREATE ACCOUNT
        //        _logger.LogInformation($"Creating an account for member");

        //        var optionsBuilder = new DbContextOptionsBuilder<EasyAccountDbContext>();
        //        optionsBuilder.UseSqlServer("YourConnectionStringHere");
        //        using (var context = new EasyAccountDbContext(optionsBuilder.Options))
        //        //LOG EXIT 
        //        {

        //            var unitOfWork = new UnitOfWork(context, _mainAccountService, _memberDetailService);
        //            var account = new MainAccountDTO
        //            {
        //                AccountName = memberDetail.MemberName,
        //                AccountType = "Member",
        //                Balance = 0,
        //                AccountNumber = accountNumber

        //            };

        //            var mappedAccount = new AutoMapper<MainAccountDTO, MainAccount>().MapToObject(account);

        //            _logger.LogInformation($"Attempting to Create an Account Record in Main Account Table");
        //            var accountCreationOutput = uni(mappedAccount);
        //            if (accountCreationOutput.IsErrorOccured)
        //            {
        //                _logger.LogInformation($"Main Account Creation Failed with Error:{accountCreationOutput.Message}");

        //            }
        //            else
        //            {
        //                //add email to Mailing List 
        //                var mailingListDetail = new MailingListDTO
        //                {
        //                    GroupId = memberDetail.GroupId,
        //                    Email = memberDetail.Email
        //                };
        //                var mailingListOutput = await _mailingListService.Create(mailingListDetail);
        //                if (mailingListOutput.IsErrorOccured)
        //                {
        //                    _logger.LogInformation($"Mailing List Entry Creation Failed with Error:{mailingListOutput.Message}");
        //                    return mailingListOutput;
        //                }
        //            }



        //            var mapped = new AutoMapper<MemberDetailDTO, MemberDetail>().MapToObject(memberDetail);
        //            //mapped.CreatedDate = DateTime.Now;
        //            //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
        //            var result = await _memberDetailService.Create(mapped);
        //            if (result.IsErrorOccured)
        //            {
        //                _logger.LogInformation($"Exited Member Creation with Error {result.Message}");

        //                throw new Exception(result.Message);
        //            }


        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        return StandardMessages.getExceptionMessage(ex);
        //    }

        //}

        private async Task<OutputHandler> GetAccountNumber(MemberDetailDTO memberDetail, string type)
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

        //Code for deleting record
        public async Task<OutputHandler> Delete(int memberDetailId)
        {

            try
            {
                await _memberDetailService.Delete(x => x.MemberId == memberDetailId);
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
            var output = await _memberDetailService.GetSingleItem(x => x.MemberId == memberDetailId);
            return new AutoMapper<MemberDetail, MemberDetailDTO>().MapToObject(output);
        }

        public async Task<IEnumerable<MemberDetailDTO>> GetAllMemberDetails()
        {
            var output = await _memberDetailService.GetAll();
            return new AutoMapper<MemberDetail, MemberDetailDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(MemberDetailDTO memberDetail)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _memberDetailService.AnyAsync(x => x.MemberId == memberDetail.MemberId);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

                    };
                }
                var mapped = new AutoMapper<MemberDetailDTO, MemberDetail>().MapToObject(memberDetail);
                //mapped.ModifiedDate = DateTime.Now;
                //mapped.ModifiedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");

                var result = await _memberDetailService.Update(mapped);
                return result;

            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }
        }

    }
}
