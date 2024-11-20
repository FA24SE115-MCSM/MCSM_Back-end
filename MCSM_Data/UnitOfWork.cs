using MCSM_Data.Entities;
using MCSM_Data.Repositories.Implementations;
using MCSM_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace MCSM_Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly McsmDbContext _context;

        private IRoleRepository _role = null!;
        private IAccountRepository _account = null!;
        private IProfileRepository _profile = null!;
        private IRoomTypeRepository _roomType = null!;
        private IRoomRepository _room = null!;
        private IRetreatRepository _retreat = null!;
        private IRetreatRegistrationRepository _retreatRegistration = null!;
        private IRetreatRegistrationParticipantRepository _retreatRegistrationParticipant = null!;
        private ILessonRepository _lesson = null!;
        private IRetreatLessonRepository _retreatLesson = null!;
        private IRetreatMonkRepository _retreatMonk = null!;
        private IRetreatGroupRepository _retreatGroup = null!;
        private IRetreatGroupMemberRepository _retreatGroupMember = null!;
        private IToolRepository _tool = null!;
        private IToolHistoryRepository _toolHistory = null!;
        private IDeviceTokenRepository _deviceToken = null!;
        private INotificationRepository _notification = null!;
        private IRetreatFileRepository _retreatFile = null!;
        private IRetreatLearningOutcomeRepository _retreatLearningOutcome = null!;
        private IRetreatScheduleRepository _retreatSchedule = null!;
        private IPaymentRepository _payment = null!;
        private IMenuRepository _menu = null!;
        private IDishRepository _dish = null!;
        private IDishTypeRepository _dishType = null!;
        private IMenuDishRepository _menuDish = null!;

        private IRefundRepository _refund = null!;

        private IFeedbackRepository _feedback = null!;


        public UnitOfWork(McsmDbContext context)
        {
            _context = context;
        }

        public IRoleRepository Role
        {
            get { return _role ??= new RoleRepository(_context); }
        }

        public IAccountRepository Account
        {
            get { return _account ??= new AccountRepository(_context); }
        }

        public IProfileRepository Profile
        {
            get { return _profile ??= new ProfileRepository(_context); }
        }

        public IRoomTypeRepository RoomType
        {
            get { return _roomType ??= new RoomTypeRepository(_context); }
        }

        public IRoomRepository Room
        {
            get { return _room ??= new RoomRepository(_context); }
        }

        public IRetreatRepository Retreat
        {
            get { return _retreat ??= new RetreatRepository(_context); }
        }

        public IRetreatRegistrationRepository RetreatRegistration
        {
            get { return _retreatRegistration ??= new RetreatRegistrationRepository(_context); }
        }

        public IRetreatRegistrationParticipantRepository RetreatRegistrationParticipant
        {
            get { return _retreatRegistrationParticipant ??= new RetreatRegistrationParticipantRepository(_context); }
        }
			
        public ILessonRepository Lesson
        {
            get { return _lesson ??= new LessonRepository(_context); }
        }

        public IRetreatLessonRepository RetreatLesson
        {
            get { return _retreatLesson ??= new RetreatLessonRepository(_context); }
        }

        public IRetreatMonkRepository RetreatMonk
        {
            get { return _retreatMonk ??= new RetreatMonkRepository(_context); }
        }

        public IRetreatGroupRepository RetreatGroup
        {
            get { return _retreatGroup ??= new RetreatGroupRepository(_context); }
        }

        public IRetreatGroupMemberRepository RetreatGroupMember
        {
            get { return _retreatGroupMember ??= new RetreatGroupMemberRepository(_context); }
        }

        public IToolRepository Tool
        {
            get { return _tool ??= new ToolRepository(_context); }
        }
        public IToolHistoryRepository ToolHistory
        {
            get { return _toolHistory ??= new ToolHistoryRepository(_context); }
        }

        public IDeviceTokenRepository DeviceToken
        {
            get { return _deviceToken ??= new DeviceTokenRepository(_context); }
        }
        public INotificationRepository Notification
        {
            get { return _notification ??= new NotificationRepository(_context); }
        }

        public IRetreatFileRepository RetreatFile
        {
            get { return _retreatFile ??= new RetreatFileRepository(_context); }
        }

        public IRetreatLearningOutcomeRepository RetreatLearningOutcome
        {
            get { return _retreatLearningOutcome ??= new RetreatLearningOutcomeRepository(_context); }
        }

        public IRetreatScheduleRepository RetreatSchedule
        {
            get { return _retreatSchedule ??= new RetreatScheduleRepository(_context); }
        }

        public IPaymentRepository Payment
        {
            get { return _payment ??= new PaymentRepository(_context); }
        }


        public IRefundRepository Refund
        {
            get { return _refund ??= new RefundRepository(_context); }
        }


        public IFeedbackRepository Feedback
        {
            get { return _feedback ??= new FeedbackRepository(_context); }
        }

        public IMenuRepository Menu
        {
            get { return _menu ??= new MenuRepository(_context); }
        }

        

        public IDishRepository Dish
        {
            get { return _dish ??= new DishRepository(_context); }
        }

        public IDishTypeRepository DishType
        {
            get { return _dishType ??= new DishTypeRepository(_context); }
        }


        public IMenuDishRepository MenuDish
        {
            get { return _menuDish ??= new MenuDishRepository(_context); }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
