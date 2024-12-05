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
    public interface IGroupScheduleService
    {
        Task<ListViewModel<GroupScheduleViewModel>> GetGroupSchedules(GroupScheduleFilterModel filter, PaginationRequestModel pagination);
        Task<GroupScheduleViewModel> GetGroupSchedule(Guid id);
        Task<GroupScheduleViewModel> CreateGroupSchedule(CreateGroupScheduleModel model);
        Task<GroupScheduleViewModel> UpdateGroupSchedule(Guid id, UpdateGroupScheduleModel model);
        Task DeleteGroupSchedule(Guid id);
    }
}
