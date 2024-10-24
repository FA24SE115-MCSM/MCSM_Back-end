using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Service.Interfaces;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using Microsoft.EntityFrameworkCore;
using MCSM_Data.Entities;
using AutoMapper.QueryableExtensions;
using MCSM_Utility.Exceptions;

namespace MCSM_Service.Implementations
{
    public class LessonService : BaseService, ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IRetreatRepository _retreatRepository;
        public LessonService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _lessonRepository = unitOfWork.Lesson;
            _retreatRepository = unitOfWork.Retreat;
        }

        public async Task<ListViewModel<LessonViewModel>> GetLessons(LessonFilterModel filter, PaginationRequestModel pagination)
        {            
            var query = _lessonRepository.GetAll();

            var db = query.Include(l => l.CreatedByNavigation)
                          .Include(l => l.CreatedByNavigation.Profile);

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = db.Where(l => l.Title.Contains(filter.Title));
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var lessons = await paginatedQuery
                .OrderBy(l => l.Title)
                .ProjectTo<LessonViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<LessonViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = lessons
            };
        }

        public async Task<LessonViewModel> GetLesson(Guid id)
        {
            return await _lessonRepository.GetMany(r => r.Id == id)
                .ProjectTo<LessonViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy lesson");
        }

        public async Task<LessonViewModel> CreateLesson(Guid accountId, CreateLessonModel model)
        {
            var lessonId = Guid.NewGuid();
            var timeNow = DateTime.UtcNow;
            var lesson = new Lesson
            {
                Id = lessonId,
                CreatedBy = accountId,
                Title = model.Title,
                Content = model.Content,
                CreateAt = timeNow,
                UpdateAt = timeNow,
                IsActive = true, // lesson's active status is default to be true
                IsDeleted = false
            };
            _lessonRepository.Add(lesson);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetLesson(lessonId) : null!;
        }

        public async Task<LessonViewModel> UpdateLesson(Guid id, UpdateLessonModel model)
        {
            var existLesson = await _lessonRepository.GetMany(r => r.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy lesson");

            existLesson.Title = model.Title ?? existLesson.Title;
            existLesson.Content = model.Content ?? existLesson.Content;
            existLesson.UpdateAt = DateTime.UtcNow;

            _lessonRepository.Update(existLesson);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetLesson(id) : null!;
        }
    }
}
