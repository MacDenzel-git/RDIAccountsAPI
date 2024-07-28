
using BusinessLogicLayer.InterestAccountsServiceContainer;
 using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestAccountController : ControllerBase
    {
        private readonly IInterestAccountService _service;
        public InterestAccountController(IInterestAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="InterestAccount"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(InterestAccountDTO interestAccount)
        {
            var outputHandler = await _service.Create(interestAccount);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="InterestAccount"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(InterestAccountDTO interestAccount)
        {
            var outputHandler = await _service.Update(interestAccount);
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

        [HttpGet("GetAllInterestAccounts")]
        public async Task<IActionResult> GetAllInterestAccounts()
        {
            var output = await _service.GetAllInterestAccounts();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="InterestAccountId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int InterestAccountId)
        {
            var output = await _service.Delete(InterestAccountId);
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

        [HttpGet("GetInterestAccount")]
        public async Task<IActionResult> GetInterestAccount(int InterestAccountId)
        {
            var output = await _service.GetInterestAccount(InterestAccountId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}