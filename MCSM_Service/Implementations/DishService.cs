using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Utility.Exceptions;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Utility.Enums;
using MCSM_Data.Models.Requests.Put;
using MCSM_Service.Interfaces;

namespace MCSM_Service.Implementations
{
    public class DishService : BaseService, IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly IDishTypeRepository _dishTypeRepository;
        private readonly IMenuDishRepository _menuDishRepository;
        public DishService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _dishRepository = unitOfWork.Dish;
            _dishTypeRepository = unitOfWork.DishType;
            _menuDishRepository = unitOfWork.MenuDish;
        }

        public async Task<ListViewModel<DishViewModel>> GetDishes(DishFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _dishRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(r => r.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.DishTypeName))
            {
                query = query.Where(r => r.DishType.Name.Contains(filter.DishTypeName));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreats = await paginatedQuery
                .ProjectTo<DishViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<DishViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreats
            };
        }

        public async Task<DishViewModel> GetDish(Guid id)
        {
            return await _dishRepository.GetMany(r => r.Id == id)
                .ProjectTo<DishViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Dish not found");
        }

        public async Task<DishViewModel> CreateDish(Guid accountId, CreateDishModel model)
        {
            var dishId = Guid.NewGuid();
            var check = _dishTypeRepository.GetMany(dt => dt.Name.Equals(model.DishTypeName)).FirstOrDefault()?.Id
                      ?? throw new NotFoundException($"Dish type '{model.DishTypeName}' not found.");

            var duplicateName = await CheckDuplicatedName(model.Name);

            if (duplicateName)
            {
                throw new BadRequestException($"Dish name {model.Name} already exist in database");
            }

            var dish = new Dish
            {
                Id = dishId,
                CreatedBy = accountId,
                DishTypeId = check,
                Name = model.Name,
                Note = model.Note,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            _dishRepository.Add(dish);
            await _unitOfWork.SaveChanges();

            return await GetDish(dishId);
        }

        public async Task<DishViewModel> UpdateDish(Guid id, UpdateDishModel model)
        {
            var existDish = await _dishRepository.GetMany(r => r.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Dish not found");

            existDish.Note = model.Note ?? existDish.Note;
            existDish.UpdateAt = DateTime.UtcNow;

            _dishRepository.Update(existDish);
            await _unitOfWork.SaveChanges();

            //_dishRepository.Update(existDish);
            //var result = await _unitOfWork.SaveChanges();
            await _unitOfWork.SaveChanges();

            return await GetDish(id);
        }

        public async Task DeleteDish(Guid id)
        {
            var menuDishes = await _menuDishRepository.GetMany(md => md.DishId == id).ToListAsync();

            if (menuDishes.Any())
            {
                foreach (var menuDish in menuDishes)
                {
                    _menuDishRepository.Remove(menuDish);
                }
                await _unitOfWork.SaveChanges();
            }

            var dish = await _dishRepository.GetMany(d => d.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy món ăn");

            _dishRepository.Remove(dish);

            await _unitOfWork.SaveChanges();
        }

        //private string GetDishStatus(string status)
        //{
        //    if (status != DishStatus.Pending.ToString() && status != DishStatus.Cooking.ToString() && status != DishStatus.Ready.ToString())
        //    {
        //        throw new BadRequestException("Invalid status. Please provide either 'Pending', 'Cooking' or 'Ready'.");
        //    }

        //    return status;
        //}

        private async Task<bool> CheckDuplicatedName(string dishName)
        {
            var dish = await _dishRepository.GetMany(d => d.Name == dishName).AsNoTracking().AnyAsync();
            return dish;
        }
    }
}
