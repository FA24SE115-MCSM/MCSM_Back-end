using MCSM_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace MCSM_Data
{
    public interface IUnitOfWork
    {

        public IRoleRepository Role { get; }
        public IAccountRepository Account { get; }
        public IProfileRepository Profile { get; }
        public IRoomTypeRepository RoomType { get; }
        public IRoomRepository Room { get; }
        public IRetreatRepository Retreat { get; }
        public IRetreatRegistrationRepository RetreatRegistration { get; }
        public IRetreatRegistrationParticipantRepository RetreatRegistrationParticipant { get; }
        public ILessonRepository Lesson { get; }
        public IRetreatLessonRepository RetreatLesson { get; }
        public IRetreatMonkRepository RetreatMonk { get; }
        public IRetreatGroupRepository RetreatGroup { get; }
        public IRetreatGroupMemberRepository RetreatGroupMember { get; }
        public IToolRepository Tool { get; }
        public IToolHistoryRepository ToolHistory { get; }
        public IDeviceTokenRepository DeviceToken { get; }
        public INotificationRepository Notification { get; }
        public IRetreatFileRepository RetreatFile { get; }
        public IRetreatLearningOutcomeRepository RetreatLearningOutcome { get; }

        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
