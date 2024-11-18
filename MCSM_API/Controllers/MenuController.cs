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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCSM_API.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        //[Authorize(AccountRole.Admin)]
        [ProducesResponseType(typeof(ListViewModel<MenuViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all menus.")]
        public async Task<ActionResult<ListViewModel<MenuViewModel>>> GetDishes([FromQuery] PaginationRequestModel pagination)
        {
            return await _menuService.GetMenus(pagination);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(MenuViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get menu by id.")]
        public async Task<ActionResult<MenuViewModel>> GetMenu([FromRoute] Guid id)
        {
            return await _menuService.GetMenu(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MenuViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create menu.")]
        public async Task<ActionResult<MenuViewModel>> CreateMenu([FromForm] CreateMenuModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var dish = await _menuService.CreateMenu(auth!.Id, model);
            return CreatedAtAction(nameof(GetMenu), new { id = dish.Id }, dish);
        }

        // PUT api/<FeedbackController>/5
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(MenuViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Update dish.")]
        public async Task<ActionResult<DishViewModel>> UpdateMenu([FromRoute] Guid id, [FromForm] UpdateMenuModel model)
        {
            var dish = await _menuService.UpdateMenu(id, model);
            return CreatedAtAction(nameof(GetMenu), new { id = dish.Id }, dish);
        }

        //// DELETE api/<MenuController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
