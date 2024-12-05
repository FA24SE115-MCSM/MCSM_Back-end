using MCSM_Data.Entities;
using MCSM_Data.Models.Views;
using MCSM_Utility.Constants;

namespace MCSM_Data.Mapping
{
    public class GeneralProfile : AutoMapper.Profile
    {
        public GeneralProfile()
        {
            CreateMap<Role, RoleViewModel>();

            CreateMap<Account, AccountViewModel>()
                .ForMember(dest => dest.Role, otp => otp.MapFrom(account => account.Role.Name))
                .ForMember(dest => dest.PhoneNumber, otp => otp.MapFrom(account => account.Profile!.PhoneNumber))
                .ForMember(dest => dest.FirstName, otp => otp.MapFrom(account => account.Profile!.FirstName))
                .ForMember(dest => dest.LastName, otp => otp.MapFrom(account => account.Profile!.LastName))
                .ForMember(dest => dest.DateOfBirth, otp => otp.MapFrom(account => account.Profile!.DateOfBirth))
                .ForMember(dest => dest.Gender, otp => otp.MapFrom(account => account.Profile!.Gender))
                .ForMember(dest => dest.Avatar, otp => otp.MapFrom(account => account.Profile!.Avatar));

            CreateMap<RoomType, RoomTypeViewModel>();
            CreateMap<Room, RoomViewModel>()
                .ForMember(dest => dest.RetreatGroupMembers, otp => otp.MapFrom(room => room.RetreatGroup!.RetreatGroupMembers));
            CreateMap<Retreat, RetreatViewModel>()
                .ForMember(dest => dest.CreatedBy, otp => otp.MapFrom(retreat => retreat.CreatedByNavigation))
                .ForMember(dest => dest.RetreatImages, otp => otp.MapFrom(retreat => retreat.RetreatFiles.Where(file => file.Type == RetreatFileType.Image)))
                .ForMember(dest => dest.RetreatDocuments, otp => otp.MapFrom(retreat => retreat.RetreatFiles.Where(file => file.Type == RetreatFileType.Document)));

            CreateMap<RetreatRegistration, RetreatRegistrationViewModel>();
            CreateMap<RetreatRegistration, ActiveRetreatRegistrationViewModel>()
                .ForMember(dest => dest.RetreatName, otp => otp.MapFrom(activeRetreat => activeRetreat.Retreat.Name))
                .ForMember(dest => dest.RetreatStatus, otp => otp.MapFrom(activeRetreat => activeRetreat.Retreat.Status))
                .ForMember(dest => dest.ParticipantEmail, otp => otp.MapFrom(activeRetreat => activeRetreat.CreateByNavigation.Email));
            //.ForMember(dest => dest.CreateBy, otp => otp.MapFrom(retreatReg => retreatReg.CreateByNavigation))
            //.ForMember(dest => dest.RetreatId, otp => otp.MapFrom(retreatReg => retreatReg.RetreatId))
            //.ForMember(dest => dest.CreateAt, otp => otp.MapFrom(retreatReg => retreatReg.CreateAt))
            //.ForMember(dest => dest.UpdateAt, otp => otp.MapFrom(retreatReg => retreatReg.UpdateAt))
            //.ForMember(dest => dest.TotalCost, otp => otp.MapFrom(retreatReg => retreatReg.TotalCost))
            //.ForMember(dest => dest.TotalParticipants, otp => otp.MapFrom(retreatReg => retreatReg.TotalParticipants))
            //.ForMember(dest => dest.IsDeleted, otp => otp.MapFrom(retreatReg => retreatReg.IsDeleted))
            //.ForMember(dest => dest.IsPaid, otp => otp.MapFrom(retreatReg => retreatReg.IsPaid));
            CreateMap<RetreatRegistrationParticipant, RetreatRegistrationParticipantViewModel>();

            CreateMap<RetreatSchedule, RetreatScheduleViewModel>()
                //.ForMember(dest => dest.GroupName, otp => otp.MapFrom(retreatSched => retreatSched.Group.Name))
                .ForMember(dest => dest.LessonTitle, otp => otp.MapFrom(retreatSched => retreatSched.RetreatLesson.Lesson.Title))
                //.ForMember(dest => dest.RoomName, otp => otp.MapFrom(retreatSched => retreatSched.UsedRoom.Name))
                .ForMember(dest => dest.LessonContent, otp => otp.MapFrom(retreatSched => retreatSched.RetreatLesson.Lesson.Content));
            CreateMap<Room, RetreatScheduleViewModel>().ForMember(dest => dest.RoomName, otp => otp.MapFrom(room => room.Name));

            CreateMap<GroupSchedule, GroupScheduleViewModel>()
                .ForMember(dest => dest.GroupName, otp => otp.MapFrom(groupSched => groupSched.Group.Name))
                .ForMember(dest => dest.RoomName, otp => otp.MapFrom(groupSched => groupSched.UsedRoom.Name))
                .ForMember(dest => dest.LessonDate, otp => otp.MapFrom(groupSched => groupSched.RetreatSchedule.LessonDate))
                .ForMember(dest => dest.LessonStart, otp => otp.MapFrom(groupSched => groupSched.RetreatSchedule.LessonStart))
                .ForMember(dest => dest.LessonEnd, otp => otp.MapFrom(groupSched => groupSched.RetreatSchedule.LessonEnd));

            CreateMap<Retreat, ProgressTrackingViewModel>()
                .ForMember(dest => dest.RetreatName, otp => otp.MapFrom(progress => progress.Name))
                .ForMember(dest => dest.RetreatId, otp => otp.MapFrom(progress => progress.Id));

            CreateMap<RetreatSchedule, RetreatScheduleViewModel>()
                .ForMember(dest => dest.GroupName, otp => otp.MapFrom(retreatSched => retreatSched.Group.Name))
                .ForMember(dest => dest.LessionTitle, otp => otp.MapFrom(retreatSched => retreatSched.RetreatLesson.Lesson.Title))
                .ForMember(dest => dest.RoomName, otp => otp.MapFrom(retreatSched => retreatSched.UsedRoom.Name))
                .ForMember(dest => dest.UsedRoom, otp => otp.MapFrom(retreatSched => retreatSched.UsedRoom));
            CreateMap<Room, RetreatScheduleViewModel>().ForMember(dest => dest.RoomName, otp => otp.MapFrom(room => room.Name));

            CreateMap<Retreat, ProgressTrackingViewModel>()
                .ForMember(dest => dest.RetreatName, otp => otp.MapFrom(progress => progress.Name))
                .ForMember(dest => dest.RetreatId, otp => otp.MapFrom(progress => progress.Id));


            CreateMap<Lesson, LessonViewModel>()
                .ForMember(dest => dest.CreatorId, otp => otp.MapFrom(lesson => lesson.CreatedByNavigation.Id))
                .ForMember(dest => dest.CreatorFirstName, otp => otp.MapFrom(lesson => lesson.CreatedByNavigation.Profile.FirstName))
                .ForMember(dest => dest.CreatorLastName, otp => otp.MapFrom(lesson => lesson.CreatedByNavigation.Profile.LastName));
            CreateMap<RetreatLesson, RetreatLessonViewModel>()
                .ForMember(dest => dest.LessonTitle, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.Title))
                .ForMember(dest => dest.AuthorId, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.CreatedByNavigation.Id))
                .ForMember(dest => dest.AuthorFirstName, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.CreatedByNavigation.Profile!.FirstName))
                .ForMember(dest => dest.AuthorLastName, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.CreatedByNavigation.Profile!.LastName));
            CreateMap<RetreatMonk, RetreatMonkViewModel>();
            CreateMap<Tool, ToolViewModel>();
            CreateMap<ToolHistory, ToolHistoryViewModel>()
                .ForMember(dest => dest.Borrower, otp => otp.MapFrom(toolHistory => toolHistory.CreatedByNavigation));
            CreateMap<Notification, NotificationViewModel>()
                .ForMember(notificationVM => notificationVM.Data, config => config.MapFrom(notification => new NotificationDataViewModel
                {
                    CreateAt = notification.CreateAt,
                    IsRead = notification.IsRead,
                    Link = notification.Link,
                    Type = notification.Type
                }));

            CreateMap<RetreatFile, RetreatImageViewModel>();
            CreateMap<RetreatFile, RetreatDocumentViewModel>();
            CreateMap<RetreatLearningOutcome, RetreatLearningOutcomeViewModel>();
            CreateMap<Payment, PaymentViewModel>()
                .ForMember(dest => dest.RetreatName, otp => otp.MapFrom(payment => payment.RetreatReg.Retreat.Name));
            CreateMap<Refund, RefundViewModel>();
            CreateMap<Feedback, FeedbackViewModel>();

            CreateMap<Menu, MenuViewModel>()
                .ForMember(dest => dest.CreatedByEmail, otp => otp.MapFrom(menu => menu.CreatedByNavigation.Email))
                .ForMember(dest => dest.Dishes, otp => otp.MapFrom(menu => menu.MenuDishes.Select(md => md.Dish)));
            CreateMap<Dish, DishViewModel>()
                .ForMember(dest => dest.CreatedByEmail, otp => otp.MapFrom(dish => dish.CreatedByNavigation.Email))
                .ForMember(dest => dest.DishTypeName, otp => otp.MapFrom(dish => dish.DishType.Name));
            CreateMap<DishType, DishTypeViewModel>();
            CreateMap<Post, PostViewModel>()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src =>
                src.Comments.Where(c => !src.Comments
                    .SelectMany(x => x.InverseParentComment)
                    .Select(x => x.Id)
                    .Contains(c.Id))
                .OrderByDescending(c => c.CreateAt)
            ));

            CreateMap<PostImage, PostImageViewModel>();
            CreateMap<Reaction, ReactionViewModel>();
            CreateMap<RetreatGroup, RetreatGroupViewModel>();
            CreateMap<RetreatGroupMember, RetreatGroupMemberViewModel>();
            CreateMap<Conversation, ConversationViewModel>();
            CreateMap<ConversationParticipant, ConversationParticipantViewModel>();
            CreateMap<Message, MessageViewModel>();

            CreateMap<Comment, CommentViewModel>();
            CreateMap<Comment, ChildCommentViewModel>();
        }
    }
}
