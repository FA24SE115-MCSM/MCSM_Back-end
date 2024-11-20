using MCSM_API.Configurations.Middleware;
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
    [Route("api/dish-type")]
    [ApiController]
    public class DishTypeController : ControllerBase
    {
        private readonly IDishTypeService _dishTypeService;
        public DishTypeController(IDishTypeService dishTypeService)
        {
            _dishTypeService = dishTypeService;
        }
        // GET: api/<DishTypeController>
        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<DishTypeViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all dish types.")]
        public async Task<ActionResult<ListViewModel<DishTypeViewModel>>> GetDishTypes([FromQuery] DishTypeFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            return await _dishTypeService.GetDishTypes(filter, pagination);
        }

        // GET api/<DishTypeController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DishTypeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get dish type by id.")]
        public async Task<ActionResult<DishTypeViewModel>> GetDishType([FromRoute] Guid id)
        {
            return await _dishTypeService.GetDishType(id);
        }

        // POST api/<DishTypeController>
        [HttpPost]
        [Authorize(AccountRole.Admin, AccountRole.Monk)]
        [ProducesResponseType(typeof(DishTypeViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create Dish Type.")]
        public async Task<ActionResult<DishTypeViewModel>> CreateFeedback([FromForm] CreateDishTypeModel model)
        {
            var dishType = await _dishTypeService.CreateDishType(model);
            return CreatedAtAction(nameof(GetDishType), new { id = dishType.Id }, dishType);
        }

        //// PUT api/<DishTypeController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DishTypeController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
