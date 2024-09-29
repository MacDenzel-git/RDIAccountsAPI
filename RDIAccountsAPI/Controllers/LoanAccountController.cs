
 using BusinessLogicLayer.Services.LoanAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanAccountController : ControllerBase
    {
        private readonly ILoanAccountService _service;
        public LoanAccountController(ILoanAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="LoanAccount"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(LoanAccountDTO loanAccount)
        {
            var outputHandler = await _service.Create(loanAccount);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="LoanAccount"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(LoanAccountDTO loanAccount)
        {
            var outputHandler = await _service.Update(loanAccount);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API that gets client Type 
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("GetAllLoanAccounts")]
        public async Task<IActionResult> GetAllLoanAccounts()
        {
            var output = await _service.GetAllLoanAccounts();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="LoanAccountId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int LoanAccountId)
        {
            var output = await _service.Delete(LoanAccountId);
            if (output.IsErrorOccured)
            {
                return BadRequest(output);
            }
            return Ok(output);
        }



        /// <summary>
        /// This is API that gets a client Type
        /// </summary>
        /// <param name="fileTypeId"></param>
        /// <returns></returns>
        /// 

        [HttpGet("GetLoanAccount")]
        public async Task<IActionResult> GetLoanAccount(int LoanAccountId)
        {
            var output = await _service.GetLoanAccount(LoanAccountId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }


        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="requestApprovalDTO"></param>
        /// <returns></returns>
        [HttpPost("ApproveLoan")]
        public async Task<IActionResult> ApproveLoan(RequestApprovalDTO requestApprovalDTO)
        {
            var outputHandler = await _service.ApproveLoan(requestApprovalDTO);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }
    }
}