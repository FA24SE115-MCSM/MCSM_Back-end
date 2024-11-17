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
    public interface IRetreatScheduleService
    {
        Task<ListViewModel<RetreatScheduleViewModel>> GetRetreatSchedulesOfARetreat(Guid retreatId, PaginationRequestModel pagination);
        Task<ListViewModel<RetreatScheduleViewModel>> GetAllRetreatSchedule(PaginationRequestModel pagination);
        Task<RetreatScheduleViewModel> GetRetreatSchedule(Guid id);
        Task<RetreatScheduleViewModel> CreateRetreatSchedule(CreateRetreatScheduleModel model);
        Task<RetreatScheduleViewModel> UpdateRetreatSchedule(Guid id, UpdateRetreatScheduleModel model);
        Task DeleteRetreatSchedule(Guid id);
    }
}
