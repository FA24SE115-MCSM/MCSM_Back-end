using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
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
    [Route("api/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        //[Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ListViewModel<DishViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all dishes.")]
        public async Task<ActionResult<ListViewModel<DishViewModel>>> GetDishes([FromQuery] DishFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _dishService.GetDishes(filter, pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get dish by id.")]
        public async Task<ActionResult<DishViewModel>> GetDish([FromRoute] Guid id)
        {
            return await _dishService.GetDish(id);
        }

        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create dish.")]
        public async Task<ActionResult<DishViewModel>> CreateDish([FromForm] CreateDishModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var dish = await _dishService.CreateDish(auth!.Id, model);
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        // PUT api/<FeedbackController>/5
        [HttpPut]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update dish.")]
        public async Task<ActionResult<DishViewModel>> UpdateFeedback([FromRoute] Guid id, [FromForm] UpdateDishModel model)
        {
            var dish = await _dishService.UpdateDish(id, model);
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        // DELETE api/<DishController>/5
        [HttpDelete]
        [Route("{id}")]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Remove dish.")]
        public async Task<IActionResult> DeleteDish([FromRoute] Guid id)
        {
            await _dishService.DeleteDish(id);
            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Món ăn đã được xóa."
            });
        }
    }
}
