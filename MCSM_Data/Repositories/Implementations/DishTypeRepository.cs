using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Data.Repositories.Implementations
{
    public class DishTypeRepository : Repository<DishType>, IDishTypeRepository
    {
        public DishTypeRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
