
 using BusinessLogicLayer.JournalEntrysServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalEntryController : ControllerBase
    {
        private readonly IJournalEntryService _service;
        public JournalEntryController(IJournalEntryService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="JournalEntry"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(JournalEntryDTO journalEntry)
        {
            var outputHandler = await _service.ShareTransaction(journalEntry);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="JournalEntry"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(JournalEntryDTO journalEntry)
        {
            var outputHandler = await _service.Update(journalEntry);
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

        [HttpGet("GetAllJournalEntrys")]
        public async Task<IActionResult> GetAllJournalEntrys()
        {
            var output = await _service.GetAllJournalEntrys();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="JournalEntryId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int JournalEntryId)
        {
            var output = await _service.Delete(JournalEntryId);
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

        [HttpGet("GetJournalEntry")]
        public async Task<IActionResult> GetJournalEntry(int JournalEntryId)
        {
            var output = await _service.GetJournalEntry(JournalEntryId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}