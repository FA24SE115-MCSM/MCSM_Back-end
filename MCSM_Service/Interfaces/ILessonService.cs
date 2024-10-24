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
    public interface ILessonService
    {
        Task<ListViewModel<LessonViewModel>> GetLessons(LessonFilterModel filter, PaginationRequestModel pagination);
        Task<LessonViewModel> GetLesson(Guid id);
        Task<LessonViewModel> CreateLesson(Guid accountId, CreateLessonModel model);
        Task<LessonViewModel> UpdateLesson(Guid id, UpdateLessonModel model);
    }
}
