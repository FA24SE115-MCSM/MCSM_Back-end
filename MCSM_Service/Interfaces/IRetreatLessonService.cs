using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRetreatLessonService
    {
        Task<ListViewModel<RetreatLessonViewModel>> GetRetreatLessonsOfARetreat(Guid retreatId, PaginationRequestModel pagination);
        Task<RetreatLessonViewModel> GetRetreatLesson(Guid id);
        Task<RetreatLessonViewModel> CreateRetreatLesson(CreateRetreatLessonModel model);
        Task DeleteRetreatLesson(Guid id);
    }
}
