using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/room-types")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        //[Authorize(AccountRole.Monks, AccountRole.Practitioners)]
        [ProducesResponseType(typeof(List<RoomTypeViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all room types.")]
        public async Task<ActionResult<List<RoomTypeViewModel>>> GetRoles()
        {
            return await _roomTypeService.GetRoomTypes();
        }
    }
}
