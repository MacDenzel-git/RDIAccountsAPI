﻿using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.Services.MailingListServiceContainer;
 using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
  using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
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
         private readonly IGroupDetailService _groupService;
        private readonly EasyAccountDbContext _context;
        private ILogger<MemberDetailService> _logger;

        private UnityOfWork _unitOfWork = new UnityOfWork();
        public MemberDetailService( 
            IGroupDetailService groupService, ILogger<MemberDetailService> logger, EasyAccountDbContext context)
        {
              _groupService = groupService;
            _logger = logger;
             _context = context;
         }
        public async Task<OutputHandler> Create(MemberDetailDTO memberDetail)
        {

 
            try
            {
                //check if record with same name already exist to avoid duplicates  - check phone number
                //bool isExist = await _memberDetailService.AnyAsync(x => x.PhoneNumber == memberDetail.PhoneNumber);
                //if (isExist)
                //{
                //    return new OutputHandler
                //    {
                //        IsErrorOccured = true,
                //        Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

                //    };
                //}

                _logger.LogInformation("Checked if Duplicate:No Duplicate");
                //Get Account nnumber
                //Compute Variables used to Create an account.
                string accountNumber = "";


                _logger.LogInformation("Started Member Creation");
                _logger.LogInformation("Create Account Number");

                var output = await GetAccountNumber(memberDetail, "member");


                if (output.IsErrorOccured)
                {
                    _logger.LogInformation($"Exited Account Generation with an Error {output.Message}");
                    //if there's an error go back
                    return new OutputHandler { IsErrorOccured = true, Message = output.Message };
                }
                else
                {
                    _logger.LogInformation($"Exited Account creation Successfully {output.Message}");

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

                var account = new MemberAccount
                {
                    MemberAccountName = memberDetail.MemberName,
                     TotalSharesContributed = 0,
                    CurrentLoanBalance = 0,
                    TotalLoans = 0,
                    TotalInterestAccumulated = 0,
                    MemberAccountNumber = accountNumber,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "LoggedInUser",
                    GroupId = memberDetail.GroupId,
                    MemberId = memberDetail.MemberId,

                };

 
                _logger.LogInformation($"Attempting to Create an Account Record in Main Account Table");
                var mainAccountResult = await _unitOfWork.MemberAccountRepository.Create(account);
                if (mainAccountResult.IsErrorOccured)
                {
                    return mainAccountResult;
                }
                else
                {
                    _logger.LogInformation($"Main Account Created");

                }

                _logger.LogInformation($"Attempting to Add Email to Mailing List Table");

                //add email to Mailing List 
                var mailingListDetail = new MailingList
                {
                    GroupId = memberDetail.GroupId,
                    Email = memberDetail.Email
                };

                var mailingListCreationResult = await _unitOfWork.MailingListRepository.Create(mailingListDetail);
                if (mailingListCreationResult.IsErrorOccured)
                {
                    _unitOfWork.RollbackTransaction();
                    return mailingListCreationResult;
                }
                else
                {
                    _logger.LogInformation($"Email added to Mailing List Successfully");

                }







                var mapped = new AutoMapper<MemberDetailDTO, MemberDetail>().MapToObject(memberDetail);
                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                _logger.LogInformation($"Attempting to add Member Details to Database");
               var memberCreationResult = await _unitOfWork.MemberRepository.Create(mapped);
                if (memberCreationResult.IsErrorOccured)
                {
                    _unitOfWork.RollbackTransaction();
                    return memberCreationResult;
                }
                else
                {
                    _logger.LogInformation($"Main Account Created");

                }

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
                        memberId = $"0{memberDetail.MemberId}";
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
                await _unitOfWork.MemberRepository.Delete(x=>x.MemberId == memberDetailId);
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
            var output = await _unitOfWork.MemberRepository.GetSingleItem(x => x.MemberId == memberDetailId);
            var mapped = new AutoMapper<MemberDetail,MemberDetailDTO>().MapToObject(output);

            return mapped;
        }

        public async Task<IEnumerable<MemberDetailDTO>> GetAllMemberDetails()
        {
            var output = await _unitOfWork.MemberRepository.GetAll();
            var mapped = new AutoMapper<MemberDetail,MemberDetailDTO>().MapToList(output);

            return mapped;
        }

        public async Task<OutputHandler> Update(MemberDetailDTO memberDetail)
        {
            try
            {
                //  check record already exist to avoid duplicates
                var isExist = await _unitOfWork.MemberRepository.AnyAsync(x => x.MemberId == memberDetail.MemberId);
                if (isExist )
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(memberDetail.PhoneNumber)

                    };
                }
                //var mapped = new AutoMapper<MemberDetailDTO, MemberDetail>().MapToObject(memberDetail);
                //mapped.ModifiedDate = DateTime.Now;
                //mapped.ModifiedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
               var mapped = new AutoMapper<MemberDetailDTO, MemberDetail>().MapToObject(memberDetail);

                var result = await _unitOfWork.MemberRepository.Update(mapped);
                return result;

            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }
        }

    }
}
