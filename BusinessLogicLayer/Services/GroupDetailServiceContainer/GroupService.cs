using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.Services.GroupAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.GroupDetailServiceContainer
{
    public class GroupDetailService : IGroupDetailService
    {

        private readonly GenericRepository<GroupDetail> _service;
        private readonly IGroupAccountService _mainAccountService;

        private ILogger<GroupDetailService> _logger;
        private UnityOfWork _unitOfWork = new UnityOfWork();

        public GroupDetailService(IGroupAccountService groupAccountService,ILogger<GroupDetailService> logger, GenericRepository<GroupDetail> service)
        {
            _service = service;
            _logger = logger;
            _mainAccountService = groupAccountService;
        }

        public async Task<OutputHandler> Create(GroupDetailDTO groupDetail)
        {


            try
            {
                //check if record with same name already exist to avoid duplicates  - check phone number
                bool isExist = await _unitOfWork.GroupDetailRepository.AnyAsync(x => x.GroupName == groupDetail.GroupName);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(groupDetail.GroupName)

                    };
                }


                isExist = await _unitOfWork.GroupDetailRepository.AnyAsync(x => x.GroupInitials == groupDetail.GroupInitials);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(groupDetail.GroupInitials)

                    };
                }


                _logger.LogInformation("Checked if Duplicate:No Duplicate- Group Name and Group Initials");
                //Get Account nnumber
                //Compute Variables used to Create an account.
                string accountNumber = "";


                _logger.LogInformation("Started Group Creation");
                _logger.LogInformation("Create Account Number");

                var output = await GetAccountNumber(groupDetail);


                if (output.IsErrorOccured)
                {
                    _logger.LogError($"Exited GroupAccount Account Creation with an Error {output.Message}");
                    //if there's an error go back
                    return new OutputHandler { IsErrorOccured = true, Message = output.Message };
                }
                else
                {
                    _logger.LogInformation($"Exited Generation creation Successfully {output.Message}");

                    accountNumber = output.Result.ToString();
                    if (accountNumber is null)
                    {
                        _logger.LogError($"Exited Account Creation successfully but account Number is Empty returning Error");

                        return new OutputHandler
                        {
                            Message = "Failed to Compute Account Number, Result returned null",
                            IsErrorOccured = true,

                        };
                    }
                }
                //Create Account 
                //LOG ENTRY CREATE ACCOUNT
                _logger.LogInformation($"Creating an account for group");

                _unitOfWork.BeginTransaction();

                var account = new GroupAccount
                {
                    GroupAccountName = groupDetail.GroupName,
                     TotalSharesReceived = 0,
                    TotalBorrowedAmount = 0,
                    InterestExpected = 0,
                    ActualInterestReceived = 0,
                    TotalLoanRepayments = 0,
                    AvailableBalance = 0,
                    GroupAccountNumber = accountNumber,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "LoggedInUser",
                    Ggbid = groupDetail.GGBId, //insert Ge=
                    GroupId = groupDetail.GroupId,
                };


                _logger.LogInformation($"Attempting to Create Group Account Record in Main Account Table");
                var mainAccountResult = await _unitOfWork.GroupAccountRepository.Create(account);
                if (mainAccountResult.IsErrorOccured)
                {
                    _logger.LogError($"Exited Group Account Account Creation with an Error {output.Message}");

                    return mainAccountResult;
                }
                else
                {
                    _logger.LogInformation($"Main Account Created");

                }

                //CREATE INTEREST ACCOUNT 
                var interestAccount = new InterestAccount
                {
                    InterestAccountNumber = $"INTACC{accountNumber}",
                    InterestAccountName = $"{groupDetail.GroupName} Interest Account ",
                    GroupId = groupDetail.GroupId ,
                    InterestAmountActualCollected = 0 ,
                    InterestAmountExpected = 0,
                };


                _logger.LogInformation($"Attempting to Create Group Interest Account Record in Interest Account Table");
                var interestAccountResult = await _unitOfWork.InterestAccountRepository.Create(interestAccount);
                if (interestAccountResult.IsErrorOccured)
                {
                    _logger.LogError($"Exited Group Interest Account Account Creation with an Error {output.Message}");

                    return interestAccountResult;
                }
                else
                {
                    _logger.LogInformation($"Group Interest Account Created");

                }





                _logger.LogInformation($"Attempting to Create Group Trans Record in Main Account Table");

                var transAccounter = new TransIdCounter
                {
                    GroupId = groupDetail.GroupId,
                    ShareTran = 1,
                    LoanTran = 2
                };
                var transResult = await _unitOfWork.TransCounterRepository.Create(transAccounter);
                if (transResult.IsErrorOccured)
                {
                    _logger.LogError($"Exited  Group Trans Record creation  with an Error {output.Message}");

                    return transResult;
                }
                else
                {
                    _logger.LogInformation($"Main Account Created");

                }




                var mapped = new AutoMapper<GroupDetailDTO, GroupDetail>().MapToObject(groupDetail);
                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                _logger.LogInformation($"Attempting to add Group Details to Database");
                var memberCreationResult = await _unitOfWork.GroupDetailRepository.Create(mapped);
                if (memberCreationResult.IsErrorOccured)
                {
                    _unitOfWork.RollbackTransaction();
                    return memberCreationResult;
                }
                else
                {
                    _logger.LogInformation($"Main Account Created");

                }


                _logger.LogInformation("Attempting to Commit Transaction Scope");
                _unitOfWork.CommitTransaction();
                _logger.LogInformation($"Transaction Scope Competed for Group {groupDetail.GroupName}-{accountNumber}");

                return new OutputHandler { IsErrorOccured = false, Message = "Successfully Completed" };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong, Attempting to Rollback ERROR:{ex}");

                _unitOfWork.RollbackTransaction();

                _logger.LogInformation($"Rollback Complete");

                return StandardMessages.getExceptionMessage(ex);
            }

        }

        private async Task<OutputHandler> GetAccountNumber(GroupDetailDTO groupDetail)
        {
            //M093NTH01DM
            //Format M=Member/ G=Group, RandomNumber-GroupInitials, 01, Member Initials 

            try
            {
                var groups = await _unitOfWork.GroupDetailRepository.GetAll();
                groupDetail.GroupId = groups.Max(x => x.GroupId);
                string accountNumber = "";

                _logger.LogInformation("Group account creation started");

                string groupId = "";
                if (groupDetail.GroupId < 10)
                {
                    groupId = $"0{groupDetail.GroupId}";
                }
                else
                {
                    groupId = groupDetail.GroupId.ToString();
                }

                //G for Group - System Group - GroupId
                accountNumber = $"G01295{groupDetail.GroupInitials}{groupDetail.GroupId}";
                _logger.LogInformation($"Account Created {accountNumber}");

                return new OutputHandler { IsErrorOccured = false, Message = "Success", Result = accountNumber };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StandardMessages.getExceptionMessage(ex);
            }
            //M093NTH01DM
        }

        public async Task<OutputHandler> Createv1(GroupDetailDTO groupDetail)
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
