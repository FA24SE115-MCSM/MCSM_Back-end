using AutoMapper;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MCSM_Service.Implementations
{
    public class RetreatRegistrationService : BaseService, IRetreatRegistrationService
    {
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        private readonly IRetreatRepository _retreatRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IAccountRepository _accountRepository;

        public RetreatRegistrationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _retreatRepository = unitOfWork.Retreat;
            _profileRepository = unitOfWork.Profile;
            _accountRepository = unitOfWork.Account;
        }

        public async Task<ListViewModel<RetreatRegistrationViewModel>> GetRetreatRegistrations(RetreatRegistrationFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _retreatRegistrationRepository.GetAll();

            if (filter.ParticipantId.HasValue)
            {
                query = query.Where(rg => rg.RetreatRegistrationParticipants.Any(rgp => rgp.ParticipantId == filter.ParticipantId.Value));
            }

            if (!string.IsNullOrEmpty(filter.RetreatName))
            {
                query = query.Where(rg => rg.Retreat.Name.Contains(filter.RetreatName));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(rg => rg.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatRegistrations = await paginatedQuery
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
                Data = retreatRegistrations
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
                //CreateBy = model.CreateBy,
                CreateBy = _accountRepository.GetMany(r => r.Email.Equals(model.CreateBy)).First().Id,
                RetreatId = _retreatRepository.GetById(model.RetreatId).Id,
                CreateAt = DateTime.UtcNow,
                TotalCost = model.TotalCost
            };
            _retreatRegistrationRepository.Add(retreatRegistration);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreatRegistration(retreatRegistrationId) : null!;
        }

        public async Task<ListViewModel<ActiveRetreatRegistrationViewModel>> GetActiveRetreatRegistrationForUser(Guid id, PaginationRequestModel pagination)
        {
            //var query = _retreatRegistrationRepository.GetMany(r => r.CreateByNavigation.Id == id && r.Retreat.Status == "Active").ProjectTo<ActiveRetreatRegistrationViewModel>(_mapper.ConfigurationProvider);
            var query = _retreatRegistrationRepository.GetMany(r => r.RetreatRegistrationParticipants.Any(p => p.ParticipantId == id) && r.Retreat.Status == "Active").OrderBy(q => q.Retreat.Name);
            var totalRow = await query.AsNoTracking().CountAsync();

            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatRegistrations = await paginatedQuery
                .ProjectTo<ActiveRetreatRegistrationViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<ActiveRetreatRegistrationViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreatRegistrations
            };
        }

        public Task CheckCapacity (Guid retreatId)
        {
            var limit = _retreatRepository.GetById(retreatId).Capacity;
            var flag = _retreatRegistrationRepository.GetMany(r => r.Id == retreatId).Sum(r => r.TotalParticipants);
            if (flag >= limit)
            {
                throw new Exception("Đã hết chỗ đăng kí.");
            }

            return Task.CompletedTask;
        }
    }
}
