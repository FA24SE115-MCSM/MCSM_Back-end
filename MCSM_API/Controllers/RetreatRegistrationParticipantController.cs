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
    [Route("api/retreat-participant")]
    [ApiController]
    public class RetreatRegistrationParticipantController : ControllerBase
    {
        private readonly IRetreatRegistrationParticipantService _retreatRegistrationParticipantService;

        public RetreatRegistrationParticipantController(IRetreatRegistrationParticipantService retreatRegistrationParticipantService)
        {
            _retreatRegistrationParticipantService = retreatRegistrationParticipantService;
        }
        // GET: api/<RetreatRegistrationParticipantController>
        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<RetreatRegistrationParticipantFilterModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all participants.")]
        public async Task<ActionResult<ListViewModel<RetreatRegistrationParticipantViewModel>>> GetRetreatRegistrationParticipants([FromQuery] RetreatRegistrationParticipantFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _retreatRegistrationParticipantService.GetRetreatRegistrationParticipants(filter, pagination);
        }

        // GET api/<RetreatRegistrationParticipantController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RetreatRegistrationParticipantViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get participant by id.")]
        public async Task<ActionResult<RetreatRegistrationParticipantViewModel>> GetRetreatRegistrationParticipant([FromRoute] Guid id)
        {
            return await _retreatRegistrationParticipantService.GetRetreatRegistrationParticipant(id);
        }

        // POST api/<RetreatRegistrationParticipantController>
        [HttpPost]
        [ProducesResponseType(typeof(RetreatRegistrationParticipantViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Add account for retreat registration.")]
        public async Task<ActionResult<RetreatRegistrationParticipantViewModel>> CreateRetreatRegistrationParticipants([FromBody] CreateRetreatRegistrationParticipantModel model)
        {
            var retreat = await _retreatRegistrationParticipantService.CreateRetreatRegistrationParticipants(model);
            //return CreatedAtAction(nameof(GetRetreatRegistrationParticipant), new { id = retreat.Id }, retreat);
            return Ok(retreat);
        }

    }
}
