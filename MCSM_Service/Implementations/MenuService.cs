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
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Utility.Enums;
using MCSM_Service.Interfaces;
using MCSM_Data.Models.Requests.Put;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MCSM_Service.Implementations
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IMenuDishRepository _menuDishRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        public MenuService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _menuRepository = unitOfWork.Menu;
            _menuDishRepository = unitOfWork.MenuDish;
            _dishRepository = unitOfWork.Dish;
        }

        public async Task<ListViewModel<MenuViewModel>> GetMenus(MenuFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _menuRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.MenuName))
            {
                if (!Enum.TryParse<MenuName>(filter.MenuName, out var name))
                {
                    throw new BadRequestException("Invalid name (Breakfast/Lunch/Dinner)");
                }
                else
                {
                    query = query.Where(m => m.MenuName.Contains(filter.MenuName));
                }
            }

            if (filter.CookDate.HasValue)
            {
                query = query.Where(m => m.CookDate == filter.CookDate);
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var menus = await paginatedQuery
                .ProjectTo<MenuViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<MenuViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = menus
            };
        }

        public async Task<MenuViewModel> GetMenu(Guid id)
        {
            return await _menuRepository.GetMany(r => r.Id == id)
                .ProjectTo<MenuViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Dish not found");
        }

        public async Task<MenuViewModel> CreateMenu(Guid accountId, CreateMenuModel model)
        {
            var menuId = Guid.NewGuid();
            if (!string.IsNullOrEmpty(model.MenuName))
            {
                if (!Enum.TryParse<MenuName>(model.MenuName, out var name))
                {
                    throw new BadRequestException("Invalid name (Breakfast/Lunch/Dinner)");
                }
            }
            var menu = new Menu
            {
                Id = menuId,
                MenuName = model.MenuName,
                CreatedBy = accountId,
                CookDate = model.CookDate,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
            };
            _menuRepository.Add(menu);
            await _unitOfWork.SaveChanges();

            foreach (var dishName in model.DishName)
            {
                var existingDish = _dishRepository.GetMany(i => i.Name.ToLower() == dishName.ToLower()).FirstOrDefault();

                if (existingDish == null)
                {
                    throw new NotFoundException($"Ingredient '{dishName}' not found.");
                }

                var menuDish = new MenuDish
                {
                    Id = Guid.NewGuid(),
                    MenuId = menuId,
                    DishId = existingDish.Id
                };

                _menuDishRepository.Add(menuDish);
            }

            await _unitOfWork.SaveChanges();

            return await GetMenu(menuId);
        }

        public async Task<MenuViewModel> UpdateMenu(Guid id, UpdateMenuModel model)
        {
            var existMenu = await _menuRepository.GetMany(r => r.Id == id)
                .Include(m => m.MenuDishes)
                .ThenInclude(md => md.Dish)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Menu not found");

            if (!string.IsNullOrEmpty(model.MenuName))
            {
                if (!Enum.TryParse<MenuName>(model.MenuName, out var name))
                {
                    throw new BadRequestException("Invalid name (Breakfast/Lunch/Dinner)");
                }
                else
                {
                    existMenu.MenuName = model.MenuName;
                }
            }

            existMenu.CookDate = model.CookDate ?? existMenu.CookDate;

            var updatedDishNames = model.DishName.Select(d => d.ToLower()).ToList();
            var existingDishIds = existMenu.MenuDishes.Select(md => md.DishId).ToList();


            foreach (var dishName in model.DishName)
            {
                var existingDish = await _dishRepository.GetMany(i => i.Name.ToLower() == dishName.ToLower())
                    .FirstOrDefaultAsync();

                if (existingDish == null)
                {
                    throw new NotFoundException($"Dish '{dishName}' not found.");
                }

                if (!existingDishIds.Contains(existingDish.Id))
                {
                    existMenu.MenuDishes.Add(new MenuDish
                    {
                        Id = Guid.NewGuid(),
                        MenuId = id,
                        DishId = existingDish.Id
                    });
                }
            }

            var dishesToRemove = existMenu.MenuDishes
                .Where(md => md.Dish != null && !updatedDishNames.Contains(md.Dish.Name.ToLower()))
                .ToList();

            foreach (var dishToRemove in dishesToRemove)
            {
                existMenu.MenuDishes.Remove(dishToRemove);
                await _unitOfWork.SaveChanges();
            }

            _menuRepository.Update(existMenu);
            await _unitOfWork.SaveChanges();

            return await GetMenu(id);
        }

        public async Task DeleteMenu(Guid id)
        {
            var menuDishes = await _menuDishRepository.GetMany(md => md.MenuId == id).ToListAsync();

            if (menuDishes.Any())
            {
                foreach (var menuDish in menuDishes)
                {
                    _menuDishRepository.Remove(menuDish);
                }
                await _unitOfWork.SaveChanges();
            }

            var menu = await _menuRepository.GetMany(d => d.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy menu");

            _menuRepository.Remove(menu);

            await _unitOfWork.SaveChanges();
        }

    }
}
