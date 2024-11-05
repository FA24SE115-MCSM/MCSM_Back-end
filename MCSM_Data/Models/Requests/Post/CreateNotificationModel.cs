using MCSM_Data.Models.Views;

namespace MCSM_Data.Models.Requests.Post
{
    public class CreateNotificationModel
    {
        public ICollection<Guid> AccountIds { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public NotificationDataViewModel Data { get; set; } = null!;
    }
}
