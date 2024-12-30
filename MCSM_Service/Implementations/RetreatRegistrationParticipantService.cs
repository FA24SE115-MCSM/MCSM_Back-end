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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var listError = new List<string>();
            var result = 0;
            var newAccounts = new List<CreateAccountModel>();
            var participants = new List<Guid>();
            var retreatReg = await _retreatRegistrationRepository.GetMany(r => r.Id == model.RetreatRegId)
                .Include(r => r.Retreat)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat Registration not found");

            if (retreatReg.Retreat.Status != RetreatStatus.Open.ToString())
            {
                throw new BadRequestException("This retreat is currently not open. Please check back later.");
            }

            if (retreatReg.IsPaid)
            {
                throw new ConflictException("Retreat Registration already paid");
            }
            if (retreatReg.IsDeleted)
            {
                throw new ConflictException("Retreat Registration is delete");
            }

            using (var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    //read list accounts form excel file
                    var listAccounts = await GetAccountFromFile(model.File);
                    //get list accounts in system
                    var accounts = await _accountRepository.GetAll().Include(acc => acc.Role).ToListAsync();

                    foreach (var account in listAccounts)
                    {
                        //check exist account
                        var existAccount = accounts.Where(x => x.Email == account.Email).FirstOrDefault();
                        if (existAccount == null)
                        {
                            //add new if not exist
                            newAccounts.Add(account);
                            continue;
                        }
                        //check account is already register retreat
                        var isAlreadyRegistered = await IsValidAccountToRegistration(existAccount, retreatReg.RetreatId);
                        if (isAlreadyRegistered)
                        {
                            continue;
                        }
                        //check overlap
                        var overlap = await CheckOverlapRetreatAsync(retreatReg.Retreat.StartDate, retreatReg.Retreat.EndDate, existAccount.Id);
                        if (overlap)
                        {
                            listError.Add($"The email: {existAccount.Email} has already been used to register for another retreat during this period.");
                            continue;
                        }
                        participants.Add(existAccount.Id);
                    }

                    //If error -> show error and stop
                    if (listError != null && listError.Count > 0)
                    {
                        throw new ReadExcelException(listError);
                    }

                    var numOfParticipants = newAccounts.Count + participants.Count;
                    if(numOfParticipants == 0)
                    {
                        throw new ConflictException("The accounts listed in the file have already registered for the retreat and cannot register again.");
                    }

                    if (numOfParticipants > retreatReg.Retreat.RemainingSlots)
                        throw new ConflictException("The number of registrants exceeded the remaining capacity of the retreat");

                    //handle register account for email not have account in system
                    var listNewAccountId = await _accountService.CreateNewAccountForRetreatRegistration(newAccounts);
                    participants.AddRange(listNewAccountId);
                    foreach (var accountId in participants)
                    {
                        var newParticipant = new RetreatRegistrationParticipant
                        {
                            Id = Guid.NewGuid(),
                            RetreatRegId = model.RetreatRegId,
                            ParticipantId = accountId,
                        };
                        _retreatRegistrationParticipantRepository.Add(newParticipant);
                    }
                    retreatReg.TotalParticipants += participants.Count;
                    retreatReg.Retreat.RemainingSlots -= participants.Count;
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

        private async Task<bool> CheckOverlapRetreatAsync(DateOnly startDate, DateOnly endDate, Guid accountId)
        {
            // Lấy danh sách RetreatRegistration của tài khoản
            var overlappingRetreats = await _retreatRegistrationRepository.GetMany(rr =>
                rr.Payments.Any(src => src.Status != PaymentStatus.Cancel.ToString()) && // Đã đăng kí
                rr.RetreatRegistrationParticipants.Any(p => p.ParticipantId == accountId) && // Account đã tham gia
                (
                    // Kiểm tra điều kiện thời gian overlap
                    (rr.Retreat.StartDate <= endDate && rr.Retreat.EndDate >= startDate)
                )
            ).ToListAsync();

            // Nếu danh sách trùng rỗng thì không bị overlap
            return overlappingRetreats.Any();
        }


        private async Task<bool> CheckAlreadyRegistered(Guid retreatId, Guid participantId)
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
            var listError = new List<string>();
            int emptyRow = 0;
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

                        var email = workSheet.Cells[row, 2].Text.ToString().Trim();
                        var firstName = workSheet.Cells[row, 3].Text.ToString().Trim();
                        var lastName = workSheet.Cells[row, 4].Text.ToString().Trim();
                        var dateOfBirth = workSheet.Cells[row, 5].Value?.ToString()?.Trim();
                        var phoneNumber = workSheet.Cells[row, 6].Text.ToString().Trim();
                        var gender = workSheet.Cells[row, 7].Text.ToString().Trim();

                        if(string.IsNullOrEmpty(email) && string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(phoneNumber) && string.IsNullOrEmpty(gender) && string.IsNullOrEmpty(dateOfBirth))
                        {
                            emptyRow++;
                            if (emptyRow == 2) break;
                            continue;
                        }

                        if (string.IsNullOrEmpty(email))
                        {
                            listError.Add($"Row {row}: Please input email");
                        }
                        if (string.IsNullOrEmpty(firstName))
                        {
                            listError.Add($"Row {row}: Please input first name");
                        }
                        if (string.IsNullOrEmpty(lastName))
                        {
                            listError.Add($"Row {row}: Please input last name");
                        }
                        if (string.IsNullOrEmpty(dateOfBirth))
                        {
                            listError.Add($"Row {row}: Please input date of birth");

                        }
                        else
                        {
                            if (!double.TryParse(dateOfBirth, out var date))
                            {
                                listError.Add($"Row {row}: Please input with format 'dd/MM/yyyy'");
                            }
                        }
                        if (string.IsNullOrEmpty(phoneNumber))
                        {
                            listError.Add($"Row {row}: Please input phone number");
                        }
                        if (string.IsNullOrEmpty(gender))
                        {
                            listError.Add($"Row {row}: Please input gender");
                        }

                        if(listError.Count == 0)
                        {
                            var account = new CreateAccountModel
                            {
                                Email = email!,
                                FirstName = firstName!,
                                LastName = lastName!,
                                DateOfBirth = DateTime.FromOADate(double.Parse(dateOfBirth)),
                                PhoneNumber = phoneNumber!,
                                Gender = gender!,
                            };
                            result.Add(account);
                        }
                    }
                }
                if (listError.Any())
                {
                    throw new ReadExcelException(listError);
                }
            }

            return result;
        }
    }
}
