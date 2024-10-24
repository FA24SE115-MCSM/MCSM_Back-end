using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using MCSM_Utility.Exceptions;
using AutoMapper.QueryableExtensions;
using MCSM_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using MCSM_Data.Models.Requests.Put;

namespace MCSM_Service.Implementations
{
    public class ProfileService : BaseService, IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IAccountRepository _accountRepository;
        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _profileRepository = unitOfWork.Profile;
            _accountRepository = unitOfWork.Account;
        }

        public async Task<ProfileViewModel> GetProfile (Guid id)
        {
            return await _profileRepository.GetMany(r => r.AccountId == id)
                .ProjectTo<ProfileViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy profile");
        }

        public async Task<ProfileViewModel> UpdateProfile(Guid id, UpdateProfileModel model)
        {
            var existProfile = await _profileRepository.GetMany(r => r.AccountId == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy profile");

            existProfile.FirstName = model.FirstName ?? existProfile.FirstName;
            existProfile.LastName = model.LastName ?? existProfile.LastName;
            existProfile.PhoneNumber = model.PhoneNumber ?? existProfile.PhoneNumber;
            existProfile.Avatar = model.Avatar ?? existProfile.Avatar;

            _profileRepository.Update(existProfile);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetProfile(id) : null!;
        }
    }
}
