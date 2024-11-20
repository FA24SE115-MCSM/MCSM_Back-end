using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Interfaces
{
    public interface IDishService
    {
        Task<ListViewModel<DishViewModel>> GetDishes(DishFilterModel filter, PaginationRequestModel pagination);
        Task<DishViewModel> GetDish(Guid id);
        Task<DishViewModel> CreateDish(Guid accountId, CreateDishModel model);
        Task<DishViewModel> UpdateDish(Guid id, UpdateDishModel model);
        Task DeleteDish(Guid id);
    }
}
