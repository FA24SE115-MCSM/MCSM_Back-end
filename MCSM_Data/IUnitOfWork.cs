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

        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
