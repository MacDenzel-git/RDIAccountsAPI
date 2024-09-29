using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.JournalEntrysServiceContainer;
using BusinessLogicLayer.Logging;
using BusinessLogicLayer.Services.JournalEntryServiceContainer;
using BusinessLogicLayer.Services.LoanAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.Extensions.Logging;
using RDIAccountsAPI;

namespace BusinessLogicLayer.Services.LoanAccountServiceContainer
{
    public class LoanAccountService : ILoanAccountService
    {

        private readonly GenericRepository<LoanAccount> _service;
        private readonly GenericRepository<AuditTrail> _auditService;
        private readonly GenericRepository<GroupAccount> _groupService;
        private readonly GenericRepository<LoanConfiguration> _loanConfigService;
        private ILogger<LoanAccountService> _logger;
        private IJournalEntryService _journalEntryService;
        private readonly IGroupDetailService _groupSetupService;
        private UnityOfWork _unitOfWork = new UnityOfWork();


        public LoanAccountService(GenericRepository<AuditTrail> auditService, GenericRepository<LoanAccount> service, GenericRepository<GroupAccount> groupService, ILogger<LoanAccountService> logger, GenericRepository<LoanConfiguration> loanConfigService, IGroupDetailService groupSetupService, IJournalEntryService journalEntryService)
        {
            _service = service;
            _groupService = groupService;
            _logger = logger;
            _loanConfigService = loanConfigService;
            _groupSetupService = groupSetupService;
            _journalEntryService = journalEntryService;
            _auditService = auditService;
        }
        public async Task<OutputHandler> Create(LoanAccountDTO loanAccount)
        {
            try
            {


                _logger.LogInformation("Starting Loan Account Creation Process");
                _logger.LogInformation(loanAccount.ToString());
                _logger.LogInformation("Checking if Member Has Loans Already");

                //check if member has an Active loan account 

                if (loanAccount.WaiveMultipleAccountsRestrictions)
                {
                    _logger.LogInformation("Loan Account Check Waived, Multiple accounts to be created");

                }
                else
                {



                    bool isExist = await _service.AnyAsync(x => x.MemberAccountNumber == loanAccount.MemberAccountNumber && x.LoanStatus == "Active");
                    if (isExist)
                    {
                        _logger.LogInformation("Loan Account Check, Account found, returning Duplicate error");

                        return new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = "Member has an a Loan Already, If you intend to still create account please click Waive Loan accounts Restriction"

                        };
                    }
                }

                //CHECK group balance 
                _logger.LogInformation("Querying Group Balance ");
                var groupCurrentBalance = await _groupService.GetSingleItem(x => x.GroupAccountId == loanAccount.GroupAccountId);
                if (groupCurrentBalance.AvailableBalance < loanAccount.RequestedLoanAmount)
                {
                    _logger.LogInformation("Error: Balance Low");

                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = "The Group does not have enough funds to process this loan"

                    };
                }

                _logger.LogInformation("Exiting Pre Checks, All Clear ");
                _logger.LogInformation("Attempting to Create Loan Account ");

                loanAccount.LoanStatus = "PENDING";
                loanAccount.LoanAcccountStatus = "PENDING";

                var memberDetail = new MemberDetailDTO
                {
                    MemberId = loanAccount.MemberId,
                    GroupId = loanAccount.GroupDetailId,

                };
                var output = await GetAccountNumber(memberDetail, "member");
                string accountNumber = "";

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
                        _logger.LogError($"ServiceName:LoanAccountService::Exited Loan Account Creation successfully but account Number is Empty returning Error SRC:LoanAccountService");

                        return new OutputHandler
                        {
                            Message = "Failed to Compute Account Number, Result returned null",
                            IsErrorOccured = true,

                        };
                    }
                    else
                    {
                        loanAccount.LoanAccountNumber = accountNumber; //save Loan account number generated
                    }
                }

                //Create Account 
                //LOG ENTRY CREATE ACCOUNT
                _logger.LogInformation($"Creating an Loan account for member");



                var loanConfiguration = await _loanConfigService.GetSingleItem(x => x.LoanConfigurationId == loanAccount.LoanConfigurationId);
                //DEFAULTS
                var mapped = new AutoMapper<LoanAccountDTO, LoanAccount>().MapToObject(loanAccount);

                //Default to Zero to not be null 
                mapped.RequestedLoanAmount = 0;
                mapped.LoanInterestAmount = 0;
                mapped.ExpectedTotalRepaymentAmount = 0;
                mapped.MonthlyInstallmentsByPeriod = 0;
                mapped.ExpectedTotalRepaymentAmount = 0;
                mapped.AmountPaid = 0;
                mapped.LoanBalance = 0;

                double interestRate = ((double)loanConfiguration.InterestRate / 100);
                var interestAmount = loanAccount.RequestedLoanAmount * interestRate;
                var principalPlusInterest = loanAccount.RequestedLoanAmount + interestAmount;
                mapped.MonthlyInstallmentsByPeriod = (double)principalPlusInterest / Convert.ToInt32(loanConfiguration.RepaymentPeriod);
                mapped.ExpectedTotalRepaymentAmount = principalPlusInterest;
                mapped.MemberAccountNumber = loanAccount.MemberAccountNumber;
                mapped.RequestedLoanAmount = loanAccount.RequestedLoanAmount;
                mapped.LoanInterestAmount = interestAmount;
                mapped.GroupId = loanAccount.GroupId;

                //mapped.MemberId = memberDetail.MemberId;

                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                var result = await _service.Create(mapped);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Happened:ServiceName:LoanAccountService:: Transaction will attempt to rollback:::ERROR:::{ex.Message}");

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
                _logger.LogInformation("Attempting to Get Group Details to Create Account Number");

                var group = await _groupSetupService.GetGroupDetail(memberDetail.GroupId);

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
                Random rnd = new Random();
                int randomNumber = rnd.Next(1000, 9999);
                accountNumber = $"L12N{randomNumber}{group.GroupInitials}95G{memberDetail.GroupId}M{memberId}"; //RK

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
        //Code for deleting record
        public async Task<OutputHandler> Delete(int loanAccountId)
        {

            try
            {
                await _service.Delete(x => x.Id == loanAccountId);
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

        public async Task<LoanAccountDTO> GetLoanAccount(int loanAccountId)
        {
            var output = await _service.GetSingleItem(x => x.Id == loanAccountId);
            return new AutoMapper<LoanAccount, LoanAccountDTO>().MapToObject(output);
        }


        public async Task<OutputHandler> ApproveLoan(RequestApprovalDTO loanAccountDTO)
        {
            try
            {
                var loanAccount = await _service.GetSingleItem(x => x.Id == loanAccountDTO.LoanAccountId);

                //Check if group has enough funds before approval
                var groupAccount = await _unitOfWork.GroupAccountRepository.GetSingleItem(x => x.GroupId == loanAccount.GroupId);

                if (groupAccount.AvailableBalance < loanAccount.RequestedLoanAmount)
                {
                    _logger.LogError("Error: Balance Low");

                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = "The Group does not have enough funds to process this loan"

                    };
                }
                var loanConfig = await _loanConfigService.GetSingleItem(x => x.LoanConfigurationId == loanAccount.LoanConfigurationId);
                _unitOfWork.BeginTransaction();
                _logger.LogInformation($"ServiceName:LoanAccountService: Loan Approval Process Started");

                //step 1 - update loan account
                loanAccount.LoanBalance = loanAccount.ExpectedTotalRepaymentAmount;
                var finalDate = DateTime.Now.Date.AddMonths(loanConfig.RepaymentPeriod);
                loanAccount.ExpectedFinalRepaymentDate = finalDate;
                loanAccount.LoanPeriod = loanConfig.RepaymentPeriod;
                loanAccount.IsApproved = true;
                loanAccount.ApprovedDate = DateTime.Now;
                loanAccount.ApprovedBy = "";
                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                loanAccount.LoanStatus = "APPROVED";
                loanAccount.LoanAcccountStatus = "ACTIVE";

                var outputHandler = await _unitOfWork.LoanAccountRepository.Update(loanAccount);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogError($"Error Happened:ServiceName:LoanAccountService:: Transaction will attempt to rollback:::ERROR:::{outputHandler.IsErrorOccured}");
                    _unitOfWork.RollbackTransaction();
                }
                else
                {
                    _logger.LogInformation($"Loan Account {loanAccount.LoanAccountNumber} was Approved By {loanAccount.ApprovedBy}");

                }
                //step 3 update member account 
                _logger.LogInformation($"step 2: Updating member account Process Started");
                var memberAccount = await _unitOfWork.MemberAccountRepository.GetSingleItem(x => x.MemberAccountNumber == loanAccount.MemberAccountNumber);
                memberAccount.TotalLoans += loanAccount.ExpectedTotalRepaymentAmount;

                //store interest accumulated
                double interestRate = ((double)loanConfig.InterestRate / 100);
                var interestAmount = loanAccount.RequestedLoanAmount * interestRate;
                memberAccount.TotalInterestAccumulated += interestAmount;
                memberAccount.CurrentLoanBalance = loanAccount.ExpectedTotalRepaymentAmount; //current loan balance set to total amount
                outputHandler = await _unitOfWork.LoanAccountRepository.Update(loanAccount);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogError($"Error Happened:ServiceName:LoanAccountService::: Transaction will attempt to rollback:::ERROR:::{outputHandler.IsErrorOccured}");
                    _unitOfWork.RollbackTransaction();
                }
                else
                {
                    _logger.LogInformation($"successfully updated Member Account");

                }

                //step 3 update group account 
                _logger.LogInformation($"step 2: Updating group account Process Started");
                groupAccount.TotalBorrowedAmount += loanAccount.ExpectedTotalRepaymentAmount;

                //store interest accumulated

                groupAccount.InterestExpected += interestAmount;
                groupAccount.OutStandingLoanAmount = loanAccount.ExpectedTotalRepaymentAmount; //current loan balance set to total amount
                groupAccount.AvailableBalance -= loanAccount.ExpectedTotalRepaymentAmount; //subtract amount from aavailable balance
                outputHandler = await _unitOfWork.LoanAccountRepository.Update(loanAccount);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogError($"Error Happened:ServiceName:LoanAccountService::: Transaction will attempt to rollback:::ERROR:::{outputHandler.IsErrorOccured}");
                    _unitOfWork.RollbackTransaction();
                }
                else
                {
                    _logger.LogInformation($"successfully updated Member Account");

                }


                _logger.LogInformation($"Loan --{loanAccount.Id}--  Approved Successfully");

                //Journal Entry for Loan dispersment
                _logger.LogInformation($"Create Journal Entry for Loan Disbersment");
                var transactionDetails = new JournalEntryDTO { };
                transactionDetails.MemberId = (int)loanAccount.MemberId;
                transactionDetails.GroupId = (int)loanAccount.GroupId;
                transactionDetails.ReceiptNo = "";
                transactionDetails.MemberName = memberAccount.MemberAccountName;
                transactionDetails.ChequeNumber = "";
                transactionDetails.AmountPaid = (double)loanAccount.ExpectedTotalRepaymentAmount;
                transactionDetails.CreatedDateTime = DateTime.UtcNow;
                transactionDetails.PayMode = "TRANSFER";
                transactionDetails.Rev = "0";
                transactionDetails.ProcessedStatus = "1";
                transactionDetails.ProcessDateTime = DateTime.UtcNow;
                transactionDetails.Processedby = "0";
                transactionDetails.TranscationTypeId = 2;
                transactionDetails.LoanBalance = (double)loanAccount.ExpectedTotalRepaymentAmount;
                transactionDetails.TranscationDetails = "Loan Disbursement";
                transactionDetails.Revreq = "0";
                transactionDetails.IsApproved = true;
                transactionDetails.ApprovedBy = "-----------";
                transactionDetails.ApprovedDate = loanAccount.ApprovedDate;
                transactionDetails.RequestMethod = "";
                transactionDetails.TransactionTypeDescription = "LOAN";
                transactionDetails.TransactionStatus = "APPROVED"; //pending, approved, completed = (monet
                transactionDetails.FromAccountNumber = "";
                transactionDetails.ToAccountNumber = "0";
                transactionDetails.MemberBankAccount = "";
                transactionDetails.MemberBankName = "";
                transactionDetails.BankTransferTransId = "";
                transactionDetails.DateLoanSent = DateTime.Now;
                transactionDetails.ModifiedBy = "------------";
                transactionDetails.DateModified = DateTime.Now;
                transactionDetails.DrCr = "";
                transactionDetails.RequestedDate = DateTime.UtcNow;
                var journalEntryOutput = await _journalEntryService.JournalEntryHandler(transactionDetails);
                if (journalEntryOutput.IsErrorOccured)
                {
                    _logger.LogInformation($"Failed to complete Journal Entry{journalEntryOutput.Message}");
                    _unitOfWork.RollbackTransaction();
                    return journalEntryOutput;
                }

                _unitOfWork.CommitTransaction();
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Loan Approved Successfully "
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return StandardMessages.getExceptionMessage(ex);
            }
        }


        public async Task<OutputHandler> RejectLoan(RequestApprovalDTO loanAccountDTO)
        {
            try
            {
                var loanAccount = await _service.GetSingleItem(x => x.Id == loanAccountDTO.LoanAccountId);

                //Check if group has enough funds before approval
                var groupAccount = await _unitOfWork.GroupAccountRepository.GetSingleItem(x => x.GroupId == loanAccount.GroupId);
                _logger.LogInformation($"ServiceName:LoanAccountService: Loan REJECTION Process Started");

                //step 1 - update loan account
                loanAccount.LoanBalance = loanAccount.ExpectedTotalRepaymentAmount;

                loanAccount.LoanPeriod = 0;
                loanAccount.IsApproved = true;
                loanAccount.ApprovedDate = DateTime.Now;
                loanAccount.ApprovedBy = "-----------";
                //mapped.CreatedDate = DateTime.Now;
                //mapped.CreatedBy = await _sessionStorage.GetItemAsync<String>("LoggedInUser");
                loanAccount.LoanStatus = "REJECTED";
                loanAccount.LoanAcccountStatus = "CLOSED";

                var outputHandler = await _service.Update(loanAccount);
                if (outputHandler.IsErrorOccured)
                {
                    _logger.LogError($"Error Happened:ServiceName:LoanAccountService:: Transaction will attempt to rollback:::ERROR:::{outputHandler.IsErrorOccured}");
                }
                else
                {
                    _logger.LogInformation($"Loan Account {loanAccount.LoanAccountNumber} was Rejected By {loanAccount.ApprovedBy}");

                }

                return outputHandler;
            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }

        }

        public async Task<IEnumerable<LoanAccountDTO>> GetAllLoanAccounts()
        {
            var output = await _service.GetAll();
            return new AutoMapper<LoanAccount, LoanAccountDTO>().MapToList(output);
        }

        public async Task<OutputHandler> Update(LoanAccountDTO loanAccount)
        {
            try
            {
                //  check record already exist to avoid duplicates
                bool isExist = await _service.AnyAsync(x => x.MemberAccountNumber == loanAccount.MemberAccountNumber);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = StandardMessages.GetDuplicateMessage(loanAccount.MemberAccountNumber)

                    };
                }
                var mapped = new AutoMapper<LoanAccountDTO, LoanAccount>().MapToObject(loanAccount);
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
