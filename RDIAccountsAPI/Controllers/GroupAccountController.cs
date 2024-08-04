
 using BusinessLogicLayer.Services.GroupAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupAccountController : ControllerBase
    {
        private readonly IGroupAccountService _service;
        public GroupAccountController(IGroupAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="GroupAccount"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(GroupAccountDTO groupAccount)
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
        /// <param name="GroupAccount"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(GroupAccountDTO groupAccount)
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

        [HttpGet("GetAllGroupAccounts")]
        public async Task<IActionResult> GetAllGroupAccounts()
        {
            var output = await _service.GetAllGroupAccounts();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="GroupAccountId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int GroupAccountId)
        {
            var output = await _service.Delete(GroupAccountId);
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

        [HttpGet("GetGroupAccount")]
        public async Task<IActionResult> GetGroupAccount(int GroupAccountId)
        {
            var output = await _service.GetGroupAccount(GroupAccountId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}