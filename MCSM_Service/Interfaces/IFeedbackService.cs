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
    public interface IFeedbackService
    {
        Task<ListViewModel<FeedbackViewModel>> GetFeedbacks(FeedbackFilterModel filter, PaginationRequestModel pagination);
        Task<FeedbackViewModel> GetFeedback(Guid id);
        Task<List<FeedbackViewModel>> GetFeedbackByAccount(Guid accountId);
        Task<List<FeedbackViewModel>> GetFeedbackByRetreat(Guid retreatId);
        Task<FeedbackViewModel> CreateFeedback(Guid accountId, CreateFeedbackModel model);
        Task<FeedbackViewModel> UpdateFeedback(Guid feedbackId, UpdateFeedbackModel model);
        Task<FeedbackViewModel> SoftDeleteFeedback(Guid feedbackId);
    }
}
