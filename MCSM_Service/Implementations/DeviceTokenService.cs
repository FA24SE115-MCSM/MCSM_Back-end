using AutoMapper;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class DeviceTokenService : BaseService, IDeviceTokenService
    {
        private readonly IDeviceTokenRepository _deviceTokenRepository;
        public DeviceTokenService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _deviceTokenRepository = unitOfWork.DeviceToken;
        }

        public async Task<bool> CreateDeviceToken(Guid accountId, CreateDeviceTokenModel model)
        {
            var deviceTokens = await _deviceTokenRepository.GetMany(token => token.AccountId.Equals(accountId)).ToListAsync();
            if (deviceTokens.Any(token => token.Token!.Equals(model.DeviceToken))) return false;
            
            _deviceTokenRepository.RemoveRange(deviceTokens);
            var newDeviceToken = new DeviceToken
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                Token = model.DeviceToken
            };

            _deviceTokenRepository.Add(newDeviceToken);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

    }
}
