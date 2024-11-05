using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Implementations
{
    public class ToolService : BaseService, IToolService
    {
        private readonly IToolRepository _toolRepository;
        private readonly ICloudStorageService _cloudStorageService;
        public ToolService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _toolRepository = unitOfWork.Tool;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<ListViewModel<ToolViewModel>> GetTools(ToolFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _toolRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(tool => tool.Name.Contains(filter.Name));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(tool => tool.Status == filter.Status.Value.ToString());
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(tool => tool.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);
            var tools = await paginatedQuery
                .ProjectTo<ToolViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<ToolViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = tools
            };
        }

        public async Task<ToolViewModel> GetTool(Guid id)
        {
            return await _toolRepository.GetMany(t => t.Id == id)
                .ProjectTo<ToolViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Tool not found");
        }

        public async Task<ToolViewModel> CreateTool(CreateToolModel model)
        {
            var toolId = Guid.NewGuid();
            var imageUrl = await UploadToolImage(toolId, model.Image, false);

            var tool = new Tool
            {
                Id = toolId,
                Name = model.Name,
                Image = imageUrl,
                TotalTool = model.TotalTool,
                Status = ToolStatus.Active.ToString()
            };
            _toolRepository.Add(tool);

            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetTool(toolId) : null!;
        }

        public async Task<ToolViewModel> UpdateTool(Guid id, UpdateToolModel model)
        {
            var tool = await _toolRepository.GetMany(t => t.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Tool not found");

            tool.Name = model.Name ?? tool.Name;
            tool.TotalTool = model.TotalTool ?? tool.TotalTool;
            tool.Status = model.Status.ToString() ?? tool.Status;

            if(model.Image != null)
            {
                var imageUrl = await UploadToolImage(id, model.Image, true);
                tool.Image = imageUrl;
            }
            _toolRepository.Update(tool);

            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetTool(id) : null!;
        }

        private async Task<string> UploadToolImage(Guid toolId, IFormFile image, bool isUpdate)
        {
            if (!image.ContentType.StartsWith("image/"))
            {
                throw new BadRequestException("The file is not an image. Please re-enter");
            }

            if (isUpdate)
            {
                await _cloudStorageService.DeleteImage(toolId);
            }

            var url = await _cloudStorageService.UploadImage(toolId, image.ContentType, image.OpenReadStream());
            return url;

        }

    }
}
