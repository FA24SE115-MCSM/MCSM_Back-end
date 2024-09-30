﻿using MCSM_Data.Entities;
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