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
    public interface IMenuService
    {
        Task<ListViewModel<MenuViewModel>> GetMenus(PaginationRequestModel pagination);
        Task<MenuViewModel> GetMenu(Guid id);
        Task<MenuViewModel> CreateMenu(Guid accountId, CreateMenuModel model);
        Task<MenuViewModel> UpdateMenu(Guid id, UpdateMenuModel model);
    }
}
