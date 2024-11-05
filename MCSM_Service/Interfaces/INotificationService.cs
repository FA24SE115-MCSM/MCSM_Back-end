using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationViewModel> GetNotification(Guid id);
        Task<ListViewModel<NotificationViewModel>> GetNotifications(Guid accountId, PaginationRequestModel pagination);
        Task<bool> SendNotification(ICollection<Guid> accountIds, CreateNotificationModel model);
        Task<NotificationViewModel> UpdateNotification(Guid id, UpdateNotificationModel model);
        Task<bool> MakeAsRead(Guid accountId);
        Task<bool> TestSendNotification(Guid accountId);
        Task<List<string>> GetDeviceToken(Guid accountId);
    }
}
