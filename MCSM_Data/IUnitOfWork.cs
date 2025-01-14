﻿using MCSM_Data.Repositories.Interfaces;
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
        public IRetreatScheduleRepository RetreatSchedule { get; }
        public IGroupScheduleRepository GroupSchedule { get; }
        public IPaymentRepository Payment { get; }
        public IRefundRepository Refund { get; }
        public IFeedbackRepository Feedback { get; }
        public IMenuRepository Menu { get; }
        public IDishRepository Dish { get; }
        public IDishTypeRepository DishType { get; }
        public IMenuDishRepository MenuDish { get; }
        public IPostRepository Post { get; }
        public IPostImageRepository PostImage { get; }
        public ICommentRepository Comment { get; }
        public IReactionRepository Reaction { get; }
        public IConversationRepository Conversation { get; }
        public IConversationParticipantRepository ConversationParticipant { get; }
        public IMessageRepository Message { get; }
        public IRetreatToolRepository RetreatTool { get; }

        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
