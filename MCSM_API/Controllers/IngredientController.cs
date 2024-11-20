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
    [Route("api/ingredient")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }
        // GET: api/<IngredientController>
        //[HttpGet]
        //[Authorize(AccountRole.Admin, AccountRole.Monk)]
        //[ProducesResponseType(typeof(ListViewModel<IngredientViewModel>), StatusCodes.Status200OK)]
        //[SwaggerOperation(Summary = "Get all ingredients.")]
        //public async Task<ActionResult<ListViewModel<IngredientViewModel>>> GetIngredients([FromQuery] IngredientFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        //{
        //    return await _ingredientService.GetIngredients(filter, pagination);
        //}

        // GET api/<IngredientController>/5
        //[HttpGet]
        //[Route("{id}")]
        //[ProducesResponseType(typeof(IngredientViewModel), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        //[SwaggerOperation(Summary = "Get ingredient by id.")]
        //public async Task<ActionResult<IngredientViewModel>> GetIngredient([FromRoute] Guid id)
        //{
        //    return await _ingredientService.GetIngredient(id);
        //}

        // POST api/<IngredientController>
        //[HttpPost]
        //[Authorize(AccountRole.Admin, AccountRole.Monk)]
        //[ProducesResponseType(typeof(IngredientViewModel), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        //[SwaggerOperation(Summary = "Create Ingredient.")]
        //public async Task<ActionResult<IngredientViewModel>> CreateIngredient([FromForm] CreateIngredientModel model)
        //{
        //    var ingredient = await _ingredientService.CreateIngredients(model);
        //    return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.Id }, ingredient);
        //}

        //// PUT api/<IngredientController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<IngredientController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
