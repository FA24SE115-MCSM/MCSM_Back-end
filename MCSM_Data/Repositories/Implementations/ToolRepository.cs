﻿using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class ToolRepository : Repository<Tool>, IToolRepository
    {
        public ToolRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
