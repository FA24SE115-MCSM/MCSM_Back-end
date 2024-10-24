using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCSM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET api/<ProfileController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProfileViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get profile by account id.")]
        public async Task<ActionResult<ProfileViewModel>> GetProfileByAccountId([FromRoute] Guid id)
        {
            return await _profileService.GetProfile(id);
        }

        // PUT api/<ProfileController>/5
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProfileViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update retreat.")]
        public async Task<ActionResult<ProfileViewModel>> UpdateProfile([FromRoute] Guid id, [FromBody] UpdateProfileModel model)
        {
            var profile = await _profileService.UpdateProfile(id, model);
            return CreatedAtAction(nameof(GetProfileByAccountId), new { id = profile.AccountId }, profile);
        }
    }
}
