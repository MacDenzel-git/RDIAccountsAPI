
using BusinessLogicLayer.GroupDetailsServiceContainer;
 using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupDetailController : ControllerBase
    {
        private readonly IGroupDetailService _service;
        public GroupDetailController(IGroupDetailService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="GroupDetail"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(GroupDetailDTO groupDetail)
        {
            var outputHandler = await _service.Create(groupDetail);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="GroupDetail"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(GroupDetailDTO groupDetail)
        {
            var outputHandler = await _service.Update(groupDetail);
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

        [HttpGet("GetAllGroupDetails")]
        public async Task<IActionResult> GetAllGroupDetails()
        {
            var output = await _service.GetAllGroupDetails();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="GroupDetailId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int GroupDetailId)
        {
            var output = await _service.Delete(GroupDetailId);
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

        [HttpGet("GetGroupDetail")]
        public async Task<IActionResult> GetGroupDetail(int GroupDetailId)
        {
            var output = await _service.GetGroupDetail(GroupDetailId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}