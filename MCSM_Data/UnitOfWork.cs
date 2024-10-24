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
