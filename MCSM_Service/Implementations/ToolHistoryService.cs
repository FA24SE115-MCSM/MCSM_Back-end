﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class ToolHistoryService : BaseService, IToolHistoryService
    {
        private readonly IToolHistoryRepository _toolHistoryRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IRetreatRepository _retreatRepository;
        public ToolHistoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _toolHistoryRepository = unitOfWork.ToolHistory;
            _toolRepository = unitOfWork.Tool;
            _retreatRepository = unitOfWork.Retreat;
        }

        public async Task<ListViewModel<ToolHistoryViewModel>> GetToolHistories(ToolHistoryFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _toolHistoryRepository.GetAll();

            if (filter.RetreatId.HasValue)
            {
                query = query.Where(t => t.RetreatId == filter.RetreatId.Value);
            }

            if (filter.ToolId.HasValue)
            {
                query = query.Where(t => t.ToolId == filter.ToolId.Value);
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(tool => tool.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);
            var toolHistories = await paginatedQuery
                .ProjectTo<ToolHistoryViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<ToolHistoryViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = toolHistories
            };
        }

        public async Task<ToolHistoryViewModel> GetToolHistory(Guid id)
        {
            return await _toolHistoryRepository.GetMany(t => t.Id == id)
                .ProjectTo<ToolHistoryViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Tool history not found");
        }

        public async Task<ToolHistoryViewModel> CreateToolHistory(Guid accountId, CreateToolHistoryModel model)
        {
            await CheckTool(model.ToolId);
            await CheckRetreat(model.RetreatId);

            var toolHistoryId = Guid.NewGuid();
            var toolHistory = new ToolHistory
            {
                Id = toolHistoryId,
                CreatedBy = accountId,
                RetreatId = model.RetreatId,
                ToolId = model.ToolId,
                NumOfTool = model.NumOfTool
            };
            _toolHistoryRepository.Add(toolHistory);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetToolHistory(toolHistoryId) : null!;
        }

        public async Task<ToolHistoryViewModel> UpdateToolHistory(Guid id, UpdateToolHistoryModel model)
        {
            var toolHistory = await _toolHistoryRepository.GetMany(t => t.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Tool history not found");

            toolHistory.NumOfTool = model.NumOfTool ?? toolHistory.NumOfTool;

            _toolHistoryRepository.Update(toolHistory);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetToolHistory(id) : null!;
        }

        private async Task CheckTool(Guid toolId)
        {
            var tool = await _toolRepository.GetMany(tool => tool.Id == toolId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Tool not found");

            if (tool.Status == ToolStatus.InActive.ToString())
            {
                throw new BadRequestException("The tool is currently InActive and cannot be used");
            }
        }
        private async Task CheckRetreat(Guid retreatId)
        {
            var retreat = await _retreatRepository.GetMany(r => r.Id == retreatId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat not found");

            if (retreat.Status == RetreatStatus.InActive.ToString())
            {
                throw new BadRequestException("The retreat is currently InActive and cannot be used.");
            }
        }
    }
}