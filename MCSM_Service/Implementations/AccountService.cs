using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using MCSM_Utility.Helpers;
using MCSM_Utility.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MCSM_Service.Implementations
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRetreatRepository _retreatRepository;
        private readonly IRetreatRegistrationRepository _retreatRegistrationRepository;

        private readonly ICloudStorageService _cloudStorageService;
        private readonly ISendMailService _sendMailService;
        private readonly AppSetting _appSettings;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings, ICloudStorageService cloudStorageService, ISendMailService sendMailService) : base(unitOfWork, mapper)
        {
            _roleRepository = unitOfWork.Role;
            _accountRepository = unitOfWork.Account;
            _profileRepository = unitOfWork.Profile;
            _retreatRepository = unitOfWork.Retreat;
            _retreatRegistrationRepository = unitOfWork.RetreatRegistration;

            _appSettings = appSettings.Value;
            _cloudStorageService = cloudStorageService;
            _sendMailService = sendMailService;
        }

        public async Task<AuthViewModel> Authenticated(AuthRequest auth)
        {
            var account = await _accountRepository.GetMany(account => account.Email == auth.Email)
                                                .Include(account => account.Role)
                                                .Include(account=> account.Profile)
                                                .FirstOrDefaultAsync();

            if (account != null && PasswordHasher.VerifyPassword(auth.Password, account.HashPassword))
            {
                if (!account.Status.Equals(AccountStatus.Active.ToString()))
                {
                    throw new BadRequestException("Your account has been locked or not activated, please contact admin to unlock it.");
                }

                var accessToken = GenerateJwtToken(new AuthModel
                {
                    Id = account.Id,
                    Role = account.Role.Name,
                    Status = account.Status
                });

                return new AuthViewModel
                {
                    AccessToken = accessToken,
                    Account = _mapper.Map<AccountViewModel>(account),
                };
            }
            throw new NotFoundException("Wrong email or password.");
        }

        public async Task<AuthModel> GetAuth(Guid id)
        {
            var auth = await _accountRepository.GetMany(account => account.Id.Equals(id))
                                                .Include(account => account.Role)
                                                .FirstOrDefaultAsync();
            if (auth != null)
            {
                return new AuthModel
                {
                    Id = auth.Id,
                    Role = auth.Role.Name,
                    Status = auth.Status
                };
            }
            throw new NotFoundException("Account not found");
        }

        public async Task<ListViewModel<AccountViewModel>> GetAccounts(AccountFilterModel filter, PaginationRequestModel pagination)
        {

            var query = _accountRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(acc => acc.Email.Contains(filter.Email));
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(acc => acc.Profile!.PhoneNumber.Contains(filter.PhoneNumber));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(acc => acc.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);
            var accounts = await paginatedQuery
                .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<AccountViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = accounts
            };
        }

        public async Task<AccountViewModel> GetAccount(Guid id)
        {
            return await _accountRepository.GetMany(account => account.Id == id)
                                            .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                                            .FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");
        }

        public async Task<AccountViewModel> CreateAccount(CreateAccountModel model)
        {
            await CheckUniqueAccount(model.Email, model.PhoneNumber);
            var gender = await CheckRoleAndGender(model.RoleId, model.Gender);


            var accountId = Guid.NewGuid();

            var profile = new MCSM_Data.Entities.Profile
            {
                AccountId = accountId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                Gender = gender,
            };
            _profileRepository.Add(profile);

            var account = new Account
            {
                Id = accountId,
                RoleId = model.RoleId,
                HashPassword = PasswordHasher.HashPassword(model.Password),
                Email = model.Email,
                Status = AccountStatus.Pending.ToString(),
                VerifyToken = CreateVerifyToken()
            };
            _accountRepository.Add(account);

            await _sendMailService.SendVerificationEmail(account.Email, account.VerifyToken);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetAccount(accountId) : null!;
        }

        public async Task<List<Guid>> CreateNewAccountForRetreatRegistration(List<CreateAccountModel> listModel)
        {
            var result = new List<Guid>();
            var participantId = await _roleRepository.GetMany(role => role.Name == AccountRole.Practitioner).Select(role => role.Id).FirstOrDefaultAsync();
            foreach (var model in listModel)
            {
                await CheckUniquePhone(model.PhoneNumber);
                model.RoleId = participantId;
                var gender = await CheckRoleAndGender(model.RoleId, model.Gender);
                var accountId = Guid.NewGuid();
                model.Password = PasswordGenerator.GenerateRandomPassword();

                var profile = new MCSM_Data.Entities.Profile
                {
                    AccountId = accountId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Gender = gender,
                };
                _profileRepository.Add(profile);

                var account = new Account
                {
                    Id = accountId,
                    RoleId = model.RoleId,
                    HashPassword = PasswordHasher.HashPassword(model.Password),
                    Email = model.Email,
                    Status = AccountStatus.Pending.ToString(),
                    VerifyToken = CreateVerifyToken()
                };
                _accountRepository.Add(account);
                result.Add(accountId);
                await _sendMailService.SendVerificationEmail(account.Email, account.VerifyToken, model.Password);
                await _unitOfWork.SaveChanges();
            }
            return result;
        }

        public async Task<AccountViewModel> UpdateAccount(Guid id, UpdateAccountModel model)
        {
            var profile = await _profileRepository.GetMany(account => account.AccountId == id).Include(profile => profile.Account).FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found.");

            if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                if (!PasswordHasher.VerifyPassword(model.OldPassword, profile.Account.HashPassword))
                {
                    throw new BadRequestException("The old password is incorrect.");
                }

                profile.Account.HashPassword = PasswordHasher.HashPassword(model.NewPassword);

            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                if(!Enum.TryParse<AccountStatus>(model.Status, out var status))
                {
                    throw new BadRequestException("Invalid account status (Active/InActive)");
                }
                profile.Account.Status = model.Status;
            }

            profile.FirstName = model.FirstName ?? profile.FirstName;
            profile.LastName = model.LastName ?? profile.LastName;
            profile.DateOfBirth = model.DateOfBirth ?? profile.DateOfBirth;
            profile.Gender = model.Gender ?? profile.Gender;
            profile.Account.UpdateAt = DateTime.UtcNow.AddHours(7);

            _profileRepository.Update(profile);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetAccount(id) : null!;
        }

        public async Task<AccountViewModel> UploadAvatar(Guid id, IFormFile image)
        {
            if (!image.ContentType.StartsWith("image/"))
            {
                throw new BadRequestException("The file is not an image. Please re-enter");
            }

            var account = await _accountRepository.GetMany(account => account.Id == id).Include(account => account.Profile).FirstOrDefaultAsync();
            if (account != null)
            {
                //xóa hình cũ trong firebase
                if (!string.IsNullOrEmpty(account.Profile!.Avatar))
                {
                    await _cloudStorageService.DeleteImage(id);
                }

                //upload hình mới
                var url = await _cloudStorageService.UploadImage(id, image.ContentType, image.OpenReadStream());

                account.Profile.Avatar = url;
                account.UpdateAt = DateTime.UtcNow.AddHours(7);

                _accountRepository.Update(account);
            }
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetAccount(id) : null!;
        }


        public async Task VerifyAccount(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new BadRequestException("Invalid token");
            }

            var account = await _accountRepository.GetMany(c => c.VerifyToken.Equals(token))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("No account found with token");

            if(account.Status != AccountStatus.Pending.ToString())
            {
                throw new BadRequestException("Account has been verified");
            }

            account.UpdateAt = DateTime.UtcNow.AddHours(7);
            account.Status = AccountStatus.Active.ToString();


            var result = await _unitOfWork.SaveChanges();

            if (result <= 0)
            {
                throw new Exception("Unable to authenticate account, please try again later.");
            }
        }

        public async Task ResetPassword(ResetPasswordModel model)
        {
            var account = await _accountRepository.GetMany(acc => acc.Email == model.Email)
                .Include(acc => acc.Profile)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("No account found with email");

            var newPassword = PasswordGenerator.GenerateRandomPassword();

            account.HashPassword = PasswordHasher.HashPassword(newPassword);
            account.UpdateAt = DateTime.UtcNow.AddHours(7);

            _accountRepository.Update(account);

            string fullName = (account.Profile?.FirstName ?? string.Empty) + " " + (account.Profile?.LastName ?? string.Empty);

            await _sendMailService.SendNewPasswordEmail(account.Email, fullName, newPassword);

            var result = await _unitOfWork.SaveChanges();

            if (result <= 0)
            {
                throw new Exception("Unable to authenticate account, please try again later.");
            }

        }


        public async Task<DharmaNameViewModel> GetDharmaName(Guid accountId, CreateDharmaNameModel model)
        {
            var retreat = await _retreatRepository.GetMany(retreat => retreat.Id == model.RetreatId).FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat not found");
            if (retreat.Status != RetreatStatus.InActive.ToString())
            {
                throw new ConflictException("The retreat is not over yet.");
            }
            var flag = await CheckAccountIsRegisteredForRetreat(model.RetreatId, accountId);
            if (!flag)
            {
                throw new ConflictException("This account is not already registered for the retreat.");
            }
            var profile = await _profileRepository.GetMany(acc => acc.AccountId == accountId).FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");

            return new DharmaNameViewModel
            {
                Name = $"{profile.FirstName} {profile.LastName}",
                DharmaName = $"{retreat.DharmaNamePrefix} {profile.LastName}",
                RetreatName = retreat.Name,
                StartDate = retreat.StartDate,
                EndDate = retreat.EndDate,
            };

        }

        //PRIVATE METHOD
        private async Task<bool> CheckAccountIsRegisteredForRetreat(Guid retreatId, Guid accountId)
        {
            return await _retreatRegistrationRepository
                .AnyAsync(retreatReg => retreatReg.RetreatId == retreatId
                                        && retreatReg.RetreatRegistrationParticipants
                                            .Any(participant => participant.ParticipantId == accountId));
        }
        private async Task<string> CheckRoleAndGender(Guid roleId, string gender)
        {
            var role = await _roleRepository.GetMany(role => role.Id == roleId)
                .FirstOrDefaultAsync() ?? throw new BadRequestException("Please re-enter roleId");

            var result = gender;

            if(role.Name == AccountRole.Monk)
            {
                result = "Male";
            }

            if(role.Name == AccountRole.Nun)
            {
                result = "FeMale";
            }

            return result;
        }

        private async Task CheckUniqueAccount(string email, string phoneNumber)
        {
            var existingAccount = await _accountRepository.GetMany(account => account.Email == email ||
                                                (account.Profile != null && account.Profile.PhoneNumber == phoneNumber))
                                        .Include(account => account.Profile)
                                        .FirstOrDefaultAsync();

            if (existingAccount != null)
            {
                
                if (existingAccount.Email == email)
                {
                    throw new BadRequestException("Email is already in use");
                }

                if (existingAccount.Profile?.PhoneNumber == phoneNumber)
                {
                    throw new BadRequestException("The phone number is already in use");
                }
            }
        }

        private async Task CheckUniquePhone(string phoneNumber)
        {
            var existingAccount = await _accountRepository.GetMany(account => account.Profile != null && account.Profile.PhoneNumber == phoneNumber)
                                        .Include(account => account.Profile)
                                        .FirstOrDefaultAsync();

            if (existingAccount != null)
            {

                if (existingAccount.Profile?.PhoneNumber == phoneNumber)
                {
                    throw new BadRequestException($"The phone number '{phoneNumber}' is already in use");
                }
            }
        }


        private string GenerateJwtToken(AuthModel auth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", auth.Id.ToString()),

                    new Claim("role", auth.Role.ToString()),

                    new Claim("status", auth.Status.ToString()),
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string CreateVerifyToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
