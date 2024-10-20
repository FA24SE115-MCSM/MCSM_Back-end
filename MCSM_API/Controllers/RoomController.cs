using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MCSM_API.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<RoomViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all rooms.")]
        public async Task<ActionResult<ListViewModel<RoomViewModel>>> GetRooms([FromQuery] RoomFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _roomService.GetRooms(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RoomViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get room by id.")]
        public async Task<ActionResult<RoomViewModel>> GetRoom([FromRoute] Guid id)
        {
            return await _roomService.GetRoom(id);
        }

       

        [HttpPost]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(RoomViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Create room.")]
        public async Task<ActionResult<RoomViewModel>> CreateRoom([FromBody] CreateRoomModel model)
        {
            var room = await _roomService.CreateRoom(model);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }



        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(RoomViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Update room.")]
        public async Task<ActionResult<RoomViewModel>> UpdateAccount([FromRoute] Guid id, [FromBody] UpdateRoomModel model)
        {
            var room = await _roomService.UpdateRoom(id, model);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }
    }
}
