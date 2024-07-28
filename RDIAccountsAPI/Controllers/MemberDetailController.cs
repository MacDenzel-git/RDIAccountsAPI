
using BusinessLogicLayer.Services.MemberDetailServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RDIAccountsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberDetailController : ControllerBase
    {
        private readonly IMemberDetailService _service;
        public MemberDetailController(IMemberDetailService service)
        {
            _service = service;
        }

        /// <summary>
        /// This is the API for creating client Type
        /// </summary>
        /// <param name="MemberDetail"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(MemberDetailDTO memberDetail)
        {
            var outputHandler = await _service.Create(memberDetail);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        /// <summary>
        /// This is the API for updating client Type
        /// </summary>
        /// <param name="MemberDetail"></param>
        /// <returns></returns>
        /// 
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MemberDetailDTO memberDetail)
        {
            var outputHandler = await _service.Update(memberDetail);
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

        [HttpGet("GetAllMemberDetails")]
        public async Task<IActionResult> GetAllMemberDetails()
        {
            var output = await _service.GetAllMemberDetails();
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();
        }

        /// <summary>
        /// This is the API that deletes a client Type
        /// </summary>
        /// <param name="MemberDetailId"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int MemberDetailId)
        {
            var output = await _service.Delete(MemberDetailId);
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

        [HttpGet("GetMemberDetail")]
        public async Task<IActionResult> GetMemberDetail(int MemberDetailId)
        {
            var output = await _service.GetMemberDetail(MemberDetailId);
            if (output != null)
            {
                return Ok(output);
            }
            return NoContent();

        }

    }
}