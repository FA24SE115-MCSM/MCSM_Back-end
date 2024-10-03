﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
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

        private readonly ICloudStorageService _cloudStorageService;
        private readonly ISendMailService _sendMailService;
        private readonly AppSetting _appSettings;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings, ICloudStorageService cloudStorageService, ISendMailService sendMailService) : base(unitOfWork, mapper)
        {
            _roleRepository = unitOfWork.Role;
            _accountRepository = unitOfWork.Account;
            _profileRepository = unitOfWork.Profile;

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
                    throw new BadRequestException("Tài khoản của bạn đã bị khóa hoặc chưa kích hoạt vui lòng liên hệ admin để mở khóa.");
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
            throw new NotFoundException("Sai tài khoản hoặc mật khẩu.");
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
            throw new NotFoundException("Không tìm thấy account.");
        }

        public async Task<List<AccountViewModel>> GetAccounts()
        {
            return await _accountRepository.GetAll()
                .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(account => account.CreateAt)
                .ToListAsync();
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
            await CheckRole(model.RoleId);


            var accountId = Guid.NewGuid();

            var profile = new MCSM_Data.Entities.Profile
            {
                AccountId = accountId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
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
            profile.Account.UpdateAt = DateTime.Now;

            _profileRepository.Update(profile);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetAccount(id) : null!;
        }

        public async Task<AccountViewModel> UploadAvatar(Guid id, IFormFile image)
        {
            var account = await _accountRepository.GetMany(account => account.Id == id).Include(account => account.Profile).FirstOrDefaultAsync();
            if (account != null)
            {
                //xóa hình cũ trong firebase
                if (!string.IsNullOrEmpty(account.Profile!.Avatar))
                {
                    await _cloudStorageService.Delete(id);
                }

                //upload hình mới
                var url = await _cloudStorageService.Upload(id, image.ContentType, image.OpenReadStream());

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
                throw new BadRequestException("Token không hợp lệ.");
            }

            var account = await _accountRepository.GetMany(c => c.VerifyToken.Equals(token))
                .FirstOrDefaultAsync() ?? throw new BadRequestException("Không tìm thấy tài khoản với token");

            if(account.Status != AccountStatus.Pending.ToString())
            {
                throw new BadRequestException("Tài khoản đã được xác thực");
            }

            account.UpdateAt = DateTime.Now;
            account.Status = AccountStatus.Active.ToString();


            var result = await _unitOfWork.SaveChanges();

            if (result <= 0)
            {
                throw new Exception("Không thể xác thực tài khoản, hãy thử lại sau.");
            }
        }

        //PRIVATE METHOD
        private async Task CheckRole(Guid roleId)
        {
            var role = await _roleRepository.GetMany(role => role.Id == roleId)
                .FirstOrDefaultAsync() ?? throw new BadRequestException("Please re-enter roleId");
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
                    throw new BadRequestException("Email đã được sử dụng");
                }

                if (existingAccount.Profile?.PhoneNumber == phoneNumber)
                {
                    throw new BadRequestException("Số điện thoại đã được sử dụng");
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
