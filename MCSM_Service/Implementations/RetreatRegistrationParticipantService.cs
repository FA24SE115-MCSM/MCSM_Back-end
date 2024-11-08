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
using MCSM_Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace MCSM_Service.Implementations
{
    public class RetreatRegistrationParticipantService : BaseService, IRetreatRegistrationParticipantService
    {
        private readonly IRetreatRegistrationParticipantRepository _retreatRegistrationParticipantRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRetreatRepository _retreatRepository;
        private readonly IAccountService _accountService;
        private readonly IRetreatRegistrationService _retreatRegistrationService;


        public RetreatRegistrationParticipantService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IRetreatRegistrationService retreatRegistrationService) : base(unitOfWork, mapper)
        {
            _retreatRegistrationParticipantRepository = unitOfWork.RetreatRegistrationParticipant;
            _profileRepository = unitOfWork.Profile;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;
            _accountRepository = unitOfWork.Account;
            _retreatRepository = unitOfWork.Retreat;    
            _accountService = accountService;
            _retreatRegistrationService = retreatRegistrationService;
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

        

        public async Task<RetreatRegistrationViewModel> CreateRetreatRegistrationParticipants(CreateRetreatRegistrationParticipantModel model)
        {
            var result = 0;
            var newAccounts = new List<CreateAccountModel>();
            var paticipants = new List<Guid>();
            var retreatReg = await _retreatRegistrationRepository.GetMany(r => r.Id == model.RetreatRegId).Include(r => r.Retreat).FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat Registration not found");
            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var listAccounts = await GetAccountFromFile(model.File);
                    foreach (var account in listAccounts)
                    {
                        var acc = await _accountRepository.GetMany(acc => acc.Email == account.Email).Include(acc => acc.Role).FirstOrDefaultAsync();
                        if(acc == null)
                        {
                            newAccounts.Add(account);
                            continue;
                        }

                        var isAlreadyRegistered = await IsValidAccountToRegistration(acc, retreatReg.RetreatId);
                        if (isAlreadyRegistered)
                        {
                            continue;
                        }
                        paticipants.Add(acc.Id);

                    }

                    var listNewAccountId = await _accountService.CreateNewAccountForRetreatRegistration(newAccounts);
                    paticipants.AddRange(listNewAccountId);
                    foreach(var accountId in paticipants)
                    {
                        var newParticipant = new RetreatRegistrationParticipant
                        {
                            Id = Guid.NewGuid(),
                            RetreatRegId = model.RetreatRegId,
                            ParticipantId = accountId,
                        };
                        _retreatRegistrationParticipantRepository.Add(newParticipant);
                    }
                    retreatReg.TotalParticipants += paticipants.Count();
                    retreatReg.TotalCost = retreatReg.TotalParticipants * retreatReg.Retreat.Cost;
                    _retreatRegistrationRepository.Update(retreatReg);
                    result = await _unitOfWork.SaveChanges();
                    
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result > 0 ? await _retreatRegistrationService.GetRetreatRegistration(model.RetreatRegId) : null!;
        }


        private async Task<bool> IsValidAccountToRegistration(Account account, Guid retreatId)
        {
            if(account.Role.Name != AccountRole.Practitioner)
            {
                throw new BadRequestException($"Email '{account.Email}' is not associated with a Practitioner account");
            }

            return await CheckAlreadyRegistered(retreatId, account.Id);
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


        private async Task<List<CreateAccountModel>> GetAccountFromFile(IFormFile file)
        {

            if (file == null || file.Length <= 0)
            {
                throw new BadRequestException("No file uploaded");
            }

            var result = new List<CreateAccountModel>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets[0];
                    var rowCount = workSheet.Dimension.Rows;
                    for (int row = 3; row <= rowCount; row++)
                    {
                        var account = new CreateAccountModel
                        {
                            Email = workSheet.Cells[row, 2].Text,
                            FirstName = workSheet.Cells[row, 3].Text,
                            LastName = workSheet.Cells[row, 4].Text,
                            DateOfBirth = DateTime.Parse(workSheet.Cells[row, 5].Text),
                            PhoneNumber = workSheet.Cells[row, 6].Text,
                            Gender = workSheet.Cells[row, 7].Text,
                        };
                        result.Add(account);
                    }
                }
            }

            return result;
        }
    }
}
