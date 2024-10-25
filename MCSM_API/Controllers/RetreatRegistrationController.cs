using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCSM_API.Controllers
{
    [Route("api/retreat-registration")]
    [ApiController]
    public class RetreatRegistrationController : ControllerBase
    {
        private readonly IRetreatRegistrationService _retreatRegistrationService;

        public RetreatRegistrationController(IRetreatRegistrationService retreatRegistrationService)
        {
            _retreatRegistrationService = retreatRegistrationService;
        }
        // GET: api/<RetreatRegistrationController>
        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<RetreatRegistrationViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all retreat registrations.")]
        public async Task<ActionResult<ListViewModel<RetreatRegistrationViewModel>>> GetRetreats([FromQuery] RetreatRegistrationFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatRegistrationService.GetRetreatRegistrations(filter, pagination);
        }

        // GET api/<RetreatRegistrationController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RetreatRegistrationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get retreat registration by id.")]
        public async Task<ActionResult<RetreatRegistrationViewModel>> GetRetreatRegistration([FromRoute] Guid id)
        {
            return await _retreatRegistrationService.GetRetreatRegistration(id);
        }

        // POST api/<RetreatRegistrationController>
        [HttpPost]
        [ProducesResponseType(typeof(RetreatRegistrationViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create retreat registrations.")]
        public async Task<ActionResult<RetreatRegistrationViewModel>> CreateRetreatRegistration([FromBody] CreateRetreatRegistrationModel model)
        {
            var retreatRegistration = await _retreatRegistrationService.CreateRetreatRegistration(model);
            return CreatedAtAction(nameof(GetRetreatRegistration), new { id = retreatRegistration.Id }, retreatRegistration);
        }

        //// PUT api/<RetreatRegistrationController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<RetreatRegistrationController>/5
        //[HttpDelete("{id}")]
        //public void DeleteImage(int id)
        //{
        //}
    }
}
