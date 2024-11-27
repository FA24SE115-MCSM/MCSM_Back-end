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
using MCSM_Utility.Constants;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using MCSM_Utility.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MCSM_Service.Implementations
{
    public class RetreatGroupService : BaseService, IRetreatGroupService
    {
        private readonly IRetreatGroupRepository _retreatGroupRepository;
        private readonly IRetreatRepository _retreatRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IRetreatGroupMemberRepository _retreatGroupMemberRepository;
        private readonly AppSetting _appSettings;
        public RetreatGroupService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _retreatGroupRepository = unitOfWork.RetreatGroup;
            _retreatRepository = unitOfWork.Retreat;
            _roomRepository = unitOfWork.Room;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _accountRepository = unitOfWork.Account;
            _roomTypeRepository = unitOfWork.RoomType;
            _retreatGroupMemberRepository = unitOfWork.RetreatGroupMember;
            _appSettings = appSettings.Value;
        }

        public async Task<ListViewModel<RetreatGroupViewModel>> GetRetreatGroups(RetreatGroupFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _retreatGroupRepository.GetAll();
            if (filter.GroupId.HasValue)
            {
                query = query.Where(r => r.Id == filter.GroupId.Value);
            }

            if (filter.RetreatId.HasValue)
            {
                query = query.Where(r => r.RetreatId == filter.RetreatId.Value);
            }

            if (filter.ParticipantId.HasValue)
            {
                query = query.Where(r => r.RetreatGroupMembers
                                            .Any(rgm => rgm.MemberId == filter.ParticipantId.Value));
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


        public async Task DivideGroup()
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));
            var retreats = await _retreatRepository.GetMany(re => re.StartDate == now.AddDays(3))
                .Include(re => re.RetreatGroups)
                .Include(re => re.RetreatRegistrations)
                .ToListAsync();

            if(retreats != null)
            {
                await DivideGroupsForRetreat(retreats);
            }
            await Task.CompletedTask;
        }


        private async Task DivideGroupsForRetreat(List<Retreat> retreats)
        {
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var bedRoomTypeId = await _roomTypeRepository
                            .GetMany(r => r.Name == "Bed room")
                            .Select(r => r.Id)
                            .FirstOrDefaultAsync();

                    foreach (var retreat in retreats)
                    {
                        if(retreat.RetreatGroups.Count > 0)
                        {
                            continue;
                        }
                        await CreateDharmaNamePrefix(retreat);
                        var participants = await _retreatRegistrationRepository.GetMany(reg => reg.RetreatId == retreat.Id).SelectMany(reg => reg.RetreatRegistrationParticipants).ToListAsync();
                        await AddParticipantToGroup(retreat.Id, participants, bedRoomTypeId);
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task CreateDharmaNamePrefix(Retreat retreat)
        {
            Random random = new Random();
            var dharmaName = _appSettings.DharmaName;
            string prefix = dharmaName.Prefixes[random.Next(dharmaName.Prefixes.Count)];

            retreat.DharmaNamePrefix = prefix;
            retreat.Status = RetreatStatus.Close.ToString();
            _retreatRepository.Update(retreat);
            await _unitOfWork.SaveChanges();
        }


        private async Task AddParticipantToGroup(Guid retreatId, List<RetreatRegistrationParticipant> participants, Guid bedRoomTypeId)
        {
            const int groupSize = 20;
            int groupNumber = 1;
            var random = new Random();
            var shuffledParticipants = participants.OrderBy(_ => random.Next()).ToList();

            for (int i = 0; i < shuffledParticipants.Count; i += groupSize)
            {
                // Lấy nhóm thiền sinh trong phòng hiện tại tối đa 20 người
                var currentGroup = shuffledParticipants.Skip(i).Take(groupSize).ToList();

                var room = new Room
                {
                    Id = Guid.NewGuid(),
                    RoomTypeId = bedRoomTypeId,
                    Name = $"Room of group {groupNumber}",
                    Capacity = groupSize,
                    Status = RoomStatus.Active.ToString()
                };
                _roomRepository.Add(room);

                var newGroup = new RetreatGroup
                {
                    Id = Guid.NewGuid(),
                    Name = $"Group {groupNumber}",
                    RetreatId = retreatId,
                    RoomId = room.Id,
                };
                _retreatGroupRepository.Add(newGroup);
                // Gán số phòng cho từng thiền sinh
                foreach (var participant in currentGroup)
                {
                    var groupMember = new RetreatGroupMember
                    {
                        Id = Guid.NewGuid(),
                        GroupId = newGroup.Id,
                        MemberId = participant.ParticipantId,
                    };
                    _retreatGroupMemberRepository.Add(groupMember);
                }

                groupNumber++;
            }
            await _unitOfWork.SaveChanges();
        }

        public async Task<RetreatGroupViewModel> AssignedMonk(CreateMonkForGroupModel model)
        {
            var retreatGroup = await _retreatGroupRepository.GetMany(r => r.Id == model.RetreatGroupId).FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat group not found");
            var flag = await _accountRepository.GetMany(a => a.Id == model.MonkId).Include(r => r.Role).FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");
            if(flag.Role.Name != AccountRole.Monk && flag.Role.Name != AccountRole.Nun)
            {
                throw new ConflictException("Please choose Monk/Nun");
            }

            retreatGroup.MonkId = model.MonkId;
            _retreatGroupRepository.Update(retreatGroup);
            var result = await _unitOfWork.SaveChanges();

            return _mapper.Map<RetreatGroupViewModel>(retreatGroup);
        }
    }
}
