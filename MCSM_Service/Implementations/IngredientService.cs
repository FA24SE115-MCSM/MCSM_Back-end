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
using MCSM_Service.Interfaces;
using AutoMapper.QueryableExtensions;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Utility.Exceptions;

namespace MCSM_Service.Implementations
{
    public class IngredientService : BaseService, IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        public IngredientService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _ingredientRepository = unitOfWork.Ingredient;
        }

        public async Task<ListViewModel<IngredientViewModel>> GetIngredients(IngredientFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _ingredientRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(l => l.Name.Contains(filter.Name));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var ingredients = await paginatedQuery
                .OrderBy(l => l.Name)
                .ProjectTo<IngredientViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<IngredientViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = ingredients
            };
        }

        public async Task<IngredientViewModel> GetIngredient(Guid id)
        {
            return await _ingredientRepository.GetMany(r => r.Id == id)
                .ProjectTo<IngredientViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy ingredient");
        }

        public async Task<IngredientViewModel> CreateIngredients(CreateIngredientModel model)
        {
            var ingredientId = Guid.NewGuid();
            var duplicateName = await CheckDuplicatedName(model.Name);
            if (duplicateName)
            {
                throw new BadRequestException($"Ingredient {model.Name} already exist.");
            }
            var ingredient = new Ingredient
            {
                Id = ingredientId,
                Name = model.Name
            };
            _ingredientRepository.Add(ingredient);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetIngredient(ingredientId) : null!;
        }

        private async Task<bool> CheckDuplicatedName(string ingredientName)
        {
            var ingredient = await _ingredientRepository.GetMany(d => d.Name == ingredientName).AsNoTracking().AnyAsync();
            return ingredient;
        }
    }
}
