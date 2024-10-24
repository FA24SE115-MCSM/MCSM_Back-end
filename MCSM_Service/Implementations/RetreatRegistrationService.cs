﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Implementations
{
    public class RetreatRegistrationService : BaseService, IRetreatRegistrationService
    {
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        private readonly IRetreatRepository _retreatRepository;
        private readonly IProfileRepository _profileRepository;

        public RetreatRegistrationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _retreatRepository = unitOfWork.Retreat;
            _profileRepository = unitOfWork.Profile;
        }

        public async Task<ListViewModel<RetreatRegistrationViewModel>> GetRetreatRegistrations(RetreatRegistrationFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _retreatRegistrationRepository.GetAll().
                Join(_retreatRepository.GetAll(),
                rr => rr.RetreatId,
                r => r.Id,
                (rr, r) => new {rr, r}
                )
                .Join(_profileRepository.GetAll(),
                combined => combined.rr.RetreatId,
                pro => pro.AccountId,
                (combined, pro) => new {combined.rr, combined.r, pro }
                );

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(q => (q.pro.FirstName + " " + q.pro.LastName).Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.RetreatName))
            {
                query = query.Where(q => q.r.Name.Contains(filter.RetreatName));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreats = await paginatedQuery
                .OrderBy(q => q.r.Name)
                .ProjectTo<RetreatRegistrationViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatRegistrationViewModel>
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

        public async Task<RetreatRegistrationViewModel> GetRetreatRegistration(Guid id)
        {
            return await _retreatRegistrationRepository.GetMany(r => r.Id == id)
                .ProjectTo<RetreatRegistrationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy đăng kí khóa thiền");
        }

        public async Task<RetreatRegistrationViewModel> CreateRetreatRegistration(CreateRetreatRegistrationModel model)
        {
            await CheckCapacity(model.RetreatId);

            var retreatRegistrationId = Guid.NewGuid();
            var retreatRegistration = new RetreatRegistration
            {
                Id = retreatRegistrationId,
                CreateBy = model.CreateBy,
                RetreatId = _retreatRepository.GetById(model.RetreatId).Id,
                CreateAt = DateTime.UtcNow,
                TotalCost = model.TotalCost
            };
            _retreatRegistrationRepository.Add(retreatRegistration);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreatRegistration(retreatRegistrationId) : null!;
        }

        public async Task CheckCapacity (Guid retreatId)
        {
            var limit = _retreatRepository.GetById(retreatId).Capacity;
            var flag = _retreatRegistrationRepository.GetMany(r => r.Id == retreatId).Sum(r => r.TotalParticipants);
            if (flag >= limit)
            {
                throw new Exception("Đã hết chỗ đăng kí.");
            }
        }

        
    }
}