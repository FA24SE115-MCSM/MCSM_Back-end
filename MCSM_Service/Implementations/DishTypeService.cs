using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Views;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Entities;
using MCSM_Data.Repositories.Implementations;
using MCSM_Utility.Exceptions;
using MCSM_Service.Interfaces;

namespace MCSM_Service.Implementations
{
    public class DishTypeService : BaseService, IDishTypeService
    {
        private readonly IDishTypeRepository _dishTypeRepository;
        public DishTypeService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _dishTypeRepository = unitOfWork.DishType;
        }

        public async Task<ListViewModel<DishTypeViewModel>> GetDishTypes(DishTypeFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _dishTypeRepository.GetAll();
            if (!string.IsNullOrEmpty(filter.DishTypeName))
            {
                query = query.Where(dt => dt.Name.Contains(filter.DishTypeName));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.Name)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var dishTypes = await paginatedQuery
                .ProjectTo<DishTypeViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<DishTypeViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = dishTypes
            };
        }

        public async Task<DishTypeViewModel> GetDishType(Guid id)
        {
            return await _dishTypeRepository.GetMany(r => r.Id == id)
                .ProjectTo<DishTypeViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Room not found");
        }

        public async Task<DishTypeViewModel> CreateDishType(CreateDishTypeModel model)
        {
            var typeId = Guid.NewGuid();

            var duplicateName = await CheckDuplicatedName(model.Name);
            if (duplicateName)
            {
                throw new BadRequestException($"Dish type {model.Name} already exist.");
            }

            var dishType = new DishType
            {
                Id = typeId,
                Name = model.Name
            };

            _dishTypeRepository.Add(dishType);
            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetDishType(typeId) : null!;
        }

        public async Task DeleteDishType(Guid id)
        {
            var dishType = await _dishTypeRepository.GetMany(dt => dt.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy loại món ăn");

            _dishTypeRepository.Remove(dishType);

            await _unitOfWork.SaveChanges();
        }


        private async Task<bool> CheckDuplicatedName(string typeName)
        {
            var type = await _dishTypeRepository.GetMany(d => d.Name == typeName).AsNoTracking().AnyAsync();
            return type;
        }
    }
}
