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
        private readonly IPaymentService _paymentService;
        private readonly AppSetting _appSettings;
        private readonly ISendMailService _sendMailService;
        public RetreatGroupService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings, IPaymentService paymentService, ISendMailService sendMailService) : base(unitOfWork, mapper)
        {
            _retreatGroupRepository = unitOfWork.RetreatGroup;
            _retreatRepository = unitOfWork.Retreat;
            _roomRepository = unitOfWork.Room;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _accountRepository = unitOfWork.Account;
            _roomTypeRepository = unitOfWork.RoomType;
            _retreatGroupMemberRepository = unitOfWork.RetreatGroupMember;
            _appSettings = appSettings.Value;
            _paymentService = paymentService;
            _sendMailService = sendMailService;
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


        public async Task HangFireDivideGroup()
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));
            var retreats = await _retreatRepository.GetMany(re => re.StartDate == now.AddDays(3))
                .Include(re => re.RetreatGroups)
                .Include(re => re.RetreatRegistrations)
                    .ThenInclude(rg => rg.CreateByNavigation)
                        .ThenInclude(a => a.Profile)
                .ToListAsync();

            if(retreats != null && retreats.Count > 0)
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
                        if(retreat.RemainingSlots > (retreat.Capacity / 2))
                        {
                            await CancelRetreat(retreat);
                            continue;
                        }
                        await CreateDharmaNamePrefix(retreat);
                        var participants = await _retreatRegistrationRepository.GetMany(reg => reg.RetreatId == retreat.Id).SelectMany(reg => reg.RetreatRegistrationParticipants).Include(reg => reg.Participant).ThenInclude(acc => acc.Profile).ToListAsync();
                        await AddParticipantToGroup(retreat, participants, bedRoomTypeId);
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

        private async Task CancelRetreat(Retreat retreat)
        {
            if(retreat.RetreatRegistrations.Count > 0)
            {
                foreach(var reg in retreat.RetreatRegistrations)
                {
                    if (reg.IsPaid)
                    {
                        var flag = await _paymentService.RefundPayment(reg.Id);
                        var fullName = $"{reg.CreateByNavigation.Profile?.FirstName} {reg.CreateByNavigation.Profile?.LastName}";
                        var totalCostFormatted = reg.TotalCost.ToString("0");
                        await _sendMailService.SendRetreatCancellationEmail(reg.CreateByNavigation.Email, fullName, totalCostFormatted);
                    }
                }
            }
            retreat.Status = RetreatStatus.InActive.ToString();
            _retreatRepository.Update(retreat);
            await _unitOfWork.SaveChanges();
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


        private async Task AddParticipantToGroup(Retreat retreat, List<RetreatRegistrationParticipant> participants, Guid bedRoomTypeId)
        {
            const int groupSize = 20;
            int groupNumber = 1;
            var random = new Random();

            // Lấy thông tin retreat
            var retreatName = retreat.Name;

            // Phân chia danh sách theo giới tính
            var maleParticipants = participants.Where(p => p.Participant.Profile!.Gender == "Male").ToList();
            var femaleParticipants = participants.Where(p => p.Participant.Profile!.Gender == "FeMale").ToList();

            // Chia nhóm cho nam
            groupNumber = AssignParticipantsToGroups(retreat.Id, retreatName, maleParticipants, bedRoomTypeId, groupNumber, random, groupSize, "Male");

            // Chia nhóm cho nữ
            groupNumber = AssignParticipantsToGroups(retreat.Id, retreatName, femaleParticipants, bedRoomTypeId, groupNumber, random, groupSize, "FeMale");

            await _unitOfWork.SaveChanges();
        }

        private int AssignParticipantsToGroups(
            Guid retreatId,
            string retreatName,
            List<RetreatRegistrationParticipant> participants,
            Guid bedRoomTypeId,
            int groupNumber,
            Random random,
            int groupSize,
            string gender)
        {
            var shuffledParticipants = participants.OrderBy(_ => random.Next()).ToList();

            for (int i = 0; i < shuffledParticipants.Count; i += groupSize)
            {
                var currentGroup = shuffledParticipants.Skip(i).Take(groupSize).ToList();

                var room = new Room
                {
                    Id = Guid.NewGuid(),
                    RoomTypeId = bedRoomTypeId,
                    Name = $"{retreatName} - Room {groupNumber} ({gender})",
                    Capacity = groupSize,
                    Status = RoomStatus.Active.ToString()
                };
                _roomRepository.Add(room);

                var newGroup = new RetreatGroup
                {
                    Id = Guid.NewGuid(),
                    Name = $"{retreatName} - Group {groupNumber} ({gender})",
                    RetreatId = retreatId,
                    RoomId = room.Id,
                };
                _retreatGroupRepository.Add(newGroup);

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

            return groupNumber;
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
