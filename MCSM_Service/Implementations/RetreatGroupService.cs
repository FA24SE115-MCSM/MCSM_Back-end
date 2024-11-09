using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Views;
using MCSM_Data.Models.Requests.Filters;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace MCSM_Service.Implementations
{
    public class RetreatGroupService : BaseService, IRetreatGroupService
    {
        private readonly IRetreatGroupRepository _retreatGroupRepository;
        public RetreatGroupService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatGroupRepository = unitOfWork.RetreatGroup;
        }

        public async Task<ListViewModel<RetreatGroupViewModel>> GetRetreatGroups(RetreatGroupFilterModel filter, PaginationViewModel pagination)
        {
            var query = _retreatGroupRepository.GetAll();
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(r => r.Name.Contains(filter.Name));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.Name)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatGroups = await paginatedQuery
                .ProjectTo<RetreatGroupViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatGroupViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreatGroups
            };
        }
    }
}
