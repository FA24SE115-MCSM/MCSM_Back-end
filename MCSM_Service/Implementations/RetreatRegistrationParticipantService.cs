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
using Org.BouncyCastle.Crypto;

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

        public async Task<ListViewModel<RetreatRegistrationParticipantViewModel>> GetRetreatRegistrationParticipants(RetreatRegistrationParticipantFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _retreatRegistrationParticipantRepository.GetAll().Include(r => r.Participant);

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<RetreatRegistrationParticipant, Account>)query.Where(r => r.Participant.Email.Contains(filter.Email));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                 .OrderBy(r => (r.Participant.Email))
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatRegistrations = await paginatedQuery
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

        public async Task<ListViewModel<RetreatRegistrationParticipantViewModel>> GetRetreatRegistratingParticipants(List<Guid> id)
        {
            var participants = await _retreatRegistrationParticipantRepository.GetMany(r => id.Contains(r.Id)).ProjectTo<RetreatRegistrationParticipantViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (participants == null || participants.Count == 0)
            {
                throw new NotFoundException("Không tìm thấy người đăng kí");
            }
            return new ListViewModel<RetreatRegistrationParticipantViewModel>
            {
                Data = participants
            };
        }

        public async Task<ListViewModel<RetreatRegistrationParticipantViewModel>> CreateRetreatRegistrationParticipants(CreateRetreatRegistrationParticipantModel model)
        {
            var participantsToAdd = new List<RetreatRegistrationParticipant>();
            var addedParticipantId = new List<Guid>();
            Guid retreatId = _retreatRegistrationRepository.GetMany(rr => rr.Id == model.retreatRegId).First().RetreatId;

            foreach (string participantEmail in model.participantEmail)
            {
                Guid participantId = _accountRepository.GetMany(a => a.Email == participantEmail).First().Id;

                bool isAlreadyRegistered = await CheckAlreadyRegistered(retreatId, participantId);

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
                addedParticipantId.Add(participant.Id);
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

                    retreatRegistration.TotalParticipants += participantsToAdd.Count;

                    await _unitOfWork.SaveChanges();
                }
            }

            return await GetRetreatRegistratingParticipants(addedParticipantId);
        }

        public async Task<bool> CheckAlreadyRegistered(Guid retreatId, Guid participantId)
        {
            var check = await _retreatRegistrationRepository.GetMany(r => r.RetreatId == retreatId)
                .Join(_retreatRegistrationParticipantRepository.GetAll(),
                rr => rr.Id,
                rrp => rrp.RetreatRegId,
                (rr, rrp) => new { rr, rrp }
                ).Where(x => x.rrp.ParticipantId == participantId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // If a record is found, it means the participant is already registered.
            return check != null;
        }

    }
}
