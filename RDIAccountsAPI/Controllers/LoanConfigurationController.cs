
using BusinessLogicLayer.Services.LoanConfigurationServiceContainer;
 using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanConfigurationController : ControllerBase
    {
        private readonly ILoanConfigurationService _service;
        public LoanConfigurationController(ILoanConfigurationService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="LoanConfiguration"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(LoanConfigurationDTO loanConfiguration)
        {
            var outputHandler = await _service.Create(loanConfiguration);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="LoanConfiguration"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(LoanConfigurationDTO loanConfiguration)
        {
            var outputHandler = await _service.Update(loanConfiguration);
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

        [HttpGet("GetAllLoanConfigurations")]
        public async Task<IActionResult> GetAllLoanConfigurations()
        {
            var output = await _service.GetAllLoanConfigurations();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="LoanConfigurationId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int LoanConfigurationId)
        {
            var output = await _service.Delete(LoanConfigurationId);
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

        [HttpGet("GetLoanConfiguration")]
        public async Task<IActionResult> GetLoanConfiguration(int LoanConfigurationId)
        {
            var output = await _service.GetLoanConfiguration(LoanConfigurationId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}