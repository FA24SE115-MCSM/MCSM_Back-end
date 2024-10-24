using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Utility.Exceptions;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Filters;

namespace MCSM_Service.Implementations
{
    public class RetreatRegistrationParticipantService : BaseService, IRetreatRegistrationParticipantService
    {
        private readonly IRetreatRegistrationParticipantRepository _retreatRegistrationParticipantRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        private readonly IAccountRepository _accountRepository;

        public RetreatRegistrationParticipantService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatRegistrationParticipantRepository = unitOfWork.RetreatRegistrationParticipant;
            _profileRepository = unitOfWork.Profile;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _accountRepository = unitOfWork.Account;
        }

        public async Task<ListViewModel<RetreatRegistrationParticipantViewModel>> GetRetreatRegistrationParticipants(RetreatRegistrationParticipantFilterModel filter,  PaginationRequestModel pagination)
        {
            var query = _retreatRegistrationParticipantRepository.GetAll()
                .Join(
                _profileRepository.GetAll(),
                rrp => rrp.ParticipantId,
                pro => pro.AccountId,
                (rrp, pro) => new {rrp, pro}
                );

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(r => (r.pro.FirstName + " " + r.pro.LastName).Contains(filter.Name));
            }



            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatRegistrations = await paginatedQuery
                .OrderBy(r => (r.pro.FirstName + " " + r.pro.LastName))
                .ProjectTo<RetreatRegistrationParticipantViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatRegistrationParticipantViewModel>
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

        public async Task<RetreatRegistrationParticipantViewModel> GetRetreatRegistrationParticipant(Guid id)
        {
            return await _retreatRegistrationParticipantRepository.GetMany(r => r.Id == id)
                .ProjectTo<RetreatRegistrationParticipantViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy người đăng kí");
        }

        //public async Task<RetreatRegistrationParticipantViewModel> CreateRetreatRegistrationParticipant(CreateRetreatRegistrationParticipantModel model)
        //{
        //    bool isAlreadyRegistered = await CheckAlreadyRegistered(model.retreatRegId, model.participantId);

        //    if (isAlreadyRegistered)
        //    {
        //        throw new BadRequestException("Participant is already registered for this retreat.");
        //    }

        //    var id = Guid.NewGuid();
        //    var participant = new RetreatRegistrationParticipant
        //    {
        //        Id = id,
        //        RetreatRegId = model.retreatRegId,
        //        ParticipantId = model.participantId
        //    };

        //    _retreatRegistrationParticipantRepository.Add(participant);

        //    var result = await _unitOfWork.SaveChanges();

        //    return result > 0 ? await GetRetreatRegistrationParticipant(id) : null!;
        //}

        public async Task<RetreatRegistrationParticipantViewModel> CreateRetreatRegistrationParticipants(CreateRetreatRegistrationParticipantModel model)
        {
            var participantsToAdd = new List<RetreatRegistrationParticipant>();
            int addedParticipantsCount = 0;

            foreach (var participantEmail in model.participantEmail)
            {
                Guid participantId = _accountRepository.GetMany(a => a.Email == participantEmail).First().Id;

                bool isAlreadyRegistered = await CheckAlreadyRegistered(model.retreatRegId, participantId);

                if (isAlreadyRegistered)
                {
                    continue;
                }

                var participant = new RetreatRegistrationParticipant
                {
                    Id = Guid.NewGuid(),
                    RetreatRegId = model.retreatRegId,
                    ParticipantId = participantId
                };

                participantsToAdd.Add(participant);
                addedParticipantsCount++;
            }

            if (participantsToAdd.Any())
            {
                _retreatRegistrationParticipantRepository.AddRange(participantsToAdd);

                var result = await _unitOfWork.SaveChanges();

                if (result > 0)
                {
                    var retreatRegistration = await _retreatRegistrationRepository
                        .GetMany(r => r.Id == model.retreatRegId)
                        .FirstOrDefaultAsync() ?? throw new BadRequestException("Retreat registration not found.");

                    retreatRegistration.TotalParticipants += addedParticipantsCount;

                    await _unitOfWork.SaveChanges();
                }
            }

            return await GetRetreatRegistrationParticipant(model.retreatRegId);
        }


        public Task<bool> CheckAlreadyRegistered(Guid retreatId, Guid participantId)
        {
            var check = _retreatRegistrationRepository.GetMany(r => r.RetreatId == retreatId)
                .Join(_retreatRegistrationParticipantRepository.GetAll(),
                rr => rr.Id,
                rrp => rrp.RetreatRegId,
                (rr, rrp) => new { rr, rrp }
                ).Where(x => x.rrp.ParticipantId == participantId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // If a record is found, it means the participant is already registered.
            return Task.FromResult(check != null);
        }

        //public Task<bool> CheckAlreadyRegistered(Guid retreatId, string participantEmail)
        //{
        //    var check = _retreatRegistrationRepository.GetMany(r => r.RetreatId == retreatId)
        //        .Join(_retreatRegistrationParticipantRepository.GetAll(),
        //        rr => rr.Id,
        //        rrp => rrp.RetreatRegId,
        //        (rr, rrp) => new { rr, rrp }
        //        )
        //        .Join(_accountRepository.GetAll(),
        //        combined => combined.rrp.ParticipantId,
        //        a => a.Id,
        //        (combined, a) => new { combined.rrp, combined.rr, a })
        //        .Where(e => e.a.Email == participantEmail)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync();

        //    return Task.FromResult(check != null);
        //}
    }
}
