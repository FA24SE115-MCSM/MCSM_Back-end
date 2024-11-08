using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
        public async Task<ActionResult<RetreatRegistrationParticipantViewModel>> CreateRetreatRegistrationParticipants([FromForm] CreateRetreatRegistrationParticipantModel model)
        {
            var retreat = await _retreatRegistrationParticipantService.CreateRetreatRegistrationParticipants(model);
            //return CreatedAtAction(nameof(GetRetreatRegistrationParticipant), new { id = retreat.Id }, retreat);
            return Ok(retreat);
        }

        [HttpPost]
        [Route("account")]
        public async Task<IActionResult> ImportAccounts(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                // Đặt LicenseContext trước khi khởi tạo ExcelPackage
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 để bỏ qua tiêu đề
                    {
                        var model = new CreateAccountModel
                        {
                            Email = worksheet.Cells[row, 1].Text,
                            Password = worksheet.Cells[row, 2].Text,
                            FirstName = worksheet.Cells[row, 3].Text,
                            LastName = worksheet.Cells[row, 4].Text,
                            DateOfBirth = DateTime.Parse(worksheet.Cells[row, 5].Text),
                            PhoneNumber = worksheet.Cells[row, 6].Text,
                            Gender = worksheet.Cells[row, 7].Text,
                            //RoleId = Guid.Parse(worksheet.Cells[row, 8].Text),
                        };

                        var name = model.FirstName;
                        //await CreateAccount(model); // Gọi phương thức lưu tài khoản
                    }


                }
            }

            return Ok("Import successful");
        }

    }
}
