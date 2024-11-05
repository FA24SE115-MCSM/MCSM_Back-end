using MCSM_Data.Models.Requests.Post;

namespace MCSM_Service.Interfaces
{
    public interface IDeviceTokenService
    {
        Task<bool> CreateDeviceToken(Guid accountId, CreateDeviceTokenModel model);
    }
}
