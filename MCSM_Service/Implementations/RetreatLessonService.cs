using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Service.Interfaces;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using Microsoft.EntityFrameworkCore;
using MCSM_Data.Entities;
using AutoMapper.QueryableExtensions;
using MCSM_Utility.Exceptions;
using MCSM_Data.Repositories.Implementations;

namespace MCSM_Service.Implementations
{
    public class RetreatLessonService : BaseService, IRetreatLessonService
    {
        private readonly IRetreatLessonRepository _retreatLessonRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IRetreatRepository _retreatRepository;
        public RetreatLessonService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatLessonRepository = unitOfWork.RetreatLesson;
            _lessonRepository = unitOfWork.Lesson;
            _retreatRepository = unitOfWork.Retreat;
        }

        public async Task<ListViewModel<RetreatLessonViewModel>> GetRetreatLessonsOfARetreat(Guid retreatId, PaginationRequestModel pagination)
        {
            var db = _retreatLessonRepository.GetAll().Include(rl => rl.Lesson)
                                                      .Include(rl => rl.Lesson.CreatedByNavigation)
                                                      .Include(rl => rl.Lesson.CreatedByNavigation.Profile);

            var query = db.Where(rl => !rl.Lesson.IsDeleted && rl.RetreatId == retreatId);

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatLessons = await paginatedQuery
                .OrderBy(r => r.Lesson.Title)
                .ProjectTo<RetreatLessonViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatLessonViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreatLessons
            };
        }

        public async Task<RetreatLessonViewModel> CreateRetreatLesson(CreateRetreatLessonModel model)
        {
            var existRetreat = await _retreatRepository.GetMany(r => r.Id == model.RetreatId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat");

            var existLesson = await _lessonRepository.GetMany(l => l.Id == model.LessonId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy lesson");

            // ### NEEDS A CHECK FOR DUPLICATED LESSON IN RETREAT

            if (existLesson.IsDeleted || !existLesson.IsActive) throw new NotFoundException("Lesson is not available!");

            var retreatLessonId = Guid.NewGuid();            
            var retreatLesson = new RetreatLesson
            {
                Id = retreatLessonId,
                RetreatId = model.RetreatId,
                LessonId = model.LessonId
            };
            _retreatLessonRepository.Add(retreatLesson);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreatLesson(retreatLessonId) : null!;
        }

        public async Task<RetreatLessonViewModel> GetRetreatLesson(Guid id)
        {
            return await _retreatLessonRepository.GetMany(rl => rl.Id == id)
                                                 .Include(rl => rl.Lesson)
                                                 .Include(rl => rl.Lesson.CreatedByNavigation)
                                                 .Include(rl => rl.Lesson.CreatedByNavigation.Profile)
                .ProjectTo<RetreatLessonViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat lesson");
        }

        public async Task DeleteRetreatLesson(Guid id)
        {
            var existRetreatLesson = await _retreatLessonRepository.GetMany(rl => rl.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat lesson");

            _retreatLessonRepository.Remove(existRetreatLesson);

            await _unitOfWork.SaveChanges();
        }
    }
}
