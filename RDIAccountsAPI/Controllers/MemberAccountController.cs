
 using BusinessLogicLayer.Services.MemberAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberAccountController : ControllerBase
    {
        private readonly IMemberAccountService _service;
        public MemberAccountController(IMemberAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="MemberAccount"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(MemberAccountDTO groupAccount)
        {
            var outputHandler = await _service.Create(groupAccount);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="MemberAccount"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MemberAccountDTO groupAccount)
        {
            var outputHandler = await _service.Update(groupAccount);
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

        [HttpGet("GetAllMemberAccounts")]
        public async Task<IActionResult> GetAllMemberAccounts()
        {
            var output = await _service.GetAllMemberAccounts();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="MemberAccountId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int MemberAccountId)
        {
            var output = await _service.Delete(MemberAccountId);
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

        [HttpGet("GetMemberAccount")]
        public async Task<IActionResult> GetMemberAccount(int MemberAccountId)
        {
            var output = await _service.GetMemberAccount(MemberAccountId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}