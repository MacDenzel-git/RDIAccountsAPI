
 using BusinessLogicLayer.Services.TransactionTypesServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _service;
        public TransactionTypeController(ITransactionTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="TransactionType"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(TransactionTypeDTO transactionType)
        {
            var outputHandler = await _service.Create(transactionType);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="TransactionType"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(TransactionTypeDTO transactionType)
        {
            var outputHandler = await _service.Update(transactionType);
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

        [HttpGet("GetAllTransactionTypes")]
        public async Task<IActionResult> GetAllTransactionTypes()
        {
            var output = await _service.GetAllTransactionTypes();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="TransactionTypeId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int TransactionTypeId)
        {
            var output = await _service.Delete(TransactionTypeId);
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

        [HttpGet("GetTransactionType")]
        public async Task<IActionResult> GetTransactionType(int TransactionTypeId)
        {
            var output = await _service.GetTransactionType(TransactionTypeId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}