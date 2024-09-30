using MCSM_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace MCSM_Data
{
    public interface IUnitOfWork
    {

        public IRoleRepository Role { get; }
        public IAccountRepository Account { get; }
        public IProfileRepository Profile { get; }

        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
