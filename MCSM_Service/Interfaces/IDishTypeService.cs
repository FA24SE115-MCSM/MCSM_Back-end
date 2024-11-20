using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Interfaces
{
    public interface IDishTypeService
    {
        Task<ListViewModel<DishTypeViewModel>> GetDishTypes(DishTypeFilterModel filter, PaginationRequestModel pagination);
        Task<DishTypeViewModel> GetDishType(Guid id);
        Task<DishTypeViewModel> CreateDishType(CreateDishTypeModel model);
        Task DeleteDishType(Guid id);
    }
}
