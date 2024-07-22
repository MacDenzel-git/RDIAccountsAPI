
 using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainAccountController : ControllerBase
    {
        private readonly IMainAccountService _service;
        public MainAccountController(IMainAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="MainAccount"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(MainAccountDTO mainAccount)
        {
            var outputHandler = await _service.Create(mainAccount);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="MainAccount"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MainAccountDTO mainAccount)
        {
            var outputHandler = await _service.Update(mainAccount);
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

        [HttpGet("GetAllMainAccounts")]
        public async Task<IActionResult> GetAllMainAccounts()
        {
            var output = await _service.GetAllMainAccounts();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="MainAccountId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int MainAccountId)
        {
            var output = await _service.Delete(MainAccountId);
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

        [HttpGet("GetMainAccount")]
        public async Task<IActionResult> GetMainAccount(int MainAccountId)
        {
            var output = await _service.GetMainAccount(MainAccountId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}