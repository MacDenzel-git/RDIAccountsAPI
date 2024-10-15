using AllinOne.DataHandlers;
using BusinessLogicLayer.Services.JournalEntryServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class JournalEntryProcessor : IJournalEntryProcessor
    {
        private UnityOfWork _unitOfWork = new UnityOfWork();
        private ILogger<JournalEntryService> _logger;

        public JournalEntryProcessor(ILogger<JournalEntryService> logger)
        {
            _logger = logger;
        }

        public async Task<OutputHandler> LoanRepaymentEntries(JournalEntryDTO entryDetails)
        {
            OutputHandler outputHandler = new OutputHandler { };
            var mappedEntryDetails = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(entryDetails);
            mappedEntryDetails.TransactionIdentifier = await GetTranId(entryDetails.GroupId, "share");
            mappedEntryDetails.TransactionIdentifier = $"SH{mappedEntryDetails.TransactionIdentifier}";
            //C
            _logger.LogInformation("attempting Double entry: C");

            //1. Debit group
            mappedEntryDetails.DrCr = "D";
            mappedEntryDetails.FromAccountNumber = entryDetails.FromAccountNumber;
            mappedEntryDetails.ToAccountNumber = entryDetails.ToAccountNumber;
            mappedEntryDetails.IsShareTransaction = false;
            mappedEntryDetails.AssociatedLoanAccountNumber = entryDetails.AssociatedLoanAccountNumber;
            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on C {outputHandler.Message}");
                return outputHandler;
            }
            _logger.LogInformation("Double entry C entered");

            //2. Credit Member
            mappedEntryDetails.DrCr = "C";

            mappedEntryDetails.JournalEntryTransId = 0; //reset tran id for next transaction
            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on D {outputHandler.Message}");
                return outputHandler;
            }

            _logger.LogInformation("Double entry D entered");
            _logger.LogInformation($"Journal Entry Transaction Completed Successfully");

            //3. Debit Loan Account
            mappedEntryDetails.DrCr = "D";

            mappedEntryDetails.JournalEntryTransId = 0; //reset tran id for next transaction
            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on D {outputHandler.Message}");
                return outputHandler;
            }

            _logger.LogInformation("Double entry D entered");
            _logger.LogInformation($"Journal Entry Transaction Completed Successfully");
            return new OutputHandler { IsErrorOccured = false, Message = "Transaction Successful" };
        }
        public async Task<OutputHandler> LoanDisburmentEntries(JournalEntryDTO entryDetails)
        {
            OutputHandler outputHandler = new OutputHandler { };
            var mappedEntryDetails = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(entryDetails);
            mappedEntryDetails.TransactionIdentifier = await GetTranId(entryDetails.GroupId, "share");
            mappedEntryDetails.TransactionIdentifier = $"LR{mappedEntryDetails.TransactionIdentifier}";
            //C
            _logger.LogInformation("attempting Double entry: C");


            _logger.LogInformation("Debit Member");
            mappedEntryDetails.DrCr = "D";
            mappedEntryDetails.FromAccountNumber = entryDetails.FromAccountNumber;
            mappedEntryDetails.IsShareTransaction = false;
            mappedEntryDetails.AssociatedLoanAccountNumber = entryDetails.AssociatedLoanAccountNumber;

            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on C {outputHandler.Message}");
                return outputHandler;
            }
            _logger.LogInformation("Double entry D entered");

            //D 
            mappedEntryDetails.DrCr = "C";
            mappedEntryDetails.JournalEntryTransId = 0; //reset tran id for next transaction
            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on D {outputHandler.Message}");
                return outputHandler;
            }

            _logger.LogInformation("Double entry D entered");
            _logger.LogInformation($"Journal Entry Transaction Completed Successfully");
            return new OutputHandler { IsErrorOccured = false, Message = "Transaction Successful" };
        }
        public async Task<OutputHandler> MemberShareJournalEntries(JournalEntryDTO entryDetails)
        {
            OutputHandler outputHandler = new OutputHandler { };
            var mappedEntryDetails = new AutoMapper<JournalEntryDTO, JournalEntry>().MapToObject(entryDetails);
            mappedEntryDetails.TransactionIdentifier = await GetTranId(entryDetails.GroupId, "share");
            mappedEntryDetails.TransactionIdentifier = $"SH{mappedEntryDetails.TransactionIdentifier}";
            //C
            _logger.LogInformation("attempting Double entry: C");

            mappedEntryDetails.DrCr = "C";
            mappedEntryDetails.FromAccountNumber = entryDetails.FromAccountNumber;
            mappedEntryDetails.IsShareTransaction = true;
            mappedEntryDetails.ToAccountNumber = entryDetails.ToAccountNumber;
            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on C {outputHandler.Message}");
                return outputHandler;
            }
            _logger.LogInformation("Double entry C entered");

            //D 
            mappedEntryDetails.DrCr = "D";

            mappedEntryDetails.JournalEntryTransId = 0; //reset tran id for next transaction
            outputHandler = await _unitOfWork.JournalEntryRepository.Create(mappedEntryDetails);
            if (outputHandler.IsErrorOccured)
            {
                _logger.LogInformation($"Double entry Failed on D {outputHandler.Message}");
                return outputHandler;
            }

            _logger.LogInformation("Double entry D entered");
            _logger.LogInformation($"Journal Entry Transaction Completed Successfully");
            return new OutputHandler { IsErrorOccured = false, Message = "Transaction Successful" };
        }

        private async Task<string> GetTranId(int groupId, string tranType)
        {
            var details = await _unitOfWork.TransCounterRepository.GetSingleItem(x => x.GroupId == groupId);
            string tranIdentifier = "";
            if (details != null)
            {

                if (tranType == "share")
                {
                    var id = details.ShareTran = +details.ShareTran;
                    tranIdentifier = id.ToString();
                    return tranIdentifier;
                }
                else
                {
                    var id = details.LoanTran = +details.LoanTran;
                    tranIdentifier = id.ToString();
                    return tranIdentifier;

                }


            }

            return "";
        }
    }
}
