using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class RetreatService : BaseService, IRetreatService
    {
        private readonly IRetreatRepository _retreatRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRetreatFileRepository _retreatFileRepository;
        private readonly IRetreatLearningOutcomeRepository _retreatLearningOutcomeRepository;
        private readonly ICloudStorageService _cloudStorageService;
        public RetreatService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _retreatRepository = unitOfWork.Retreat;
            _accountRepository = unitOfWork.Account;
            _retreatFileRepository = unitOfWork.RetreatFile;
            _retreatLearningOutcomeRepository = unitOfWork.RetreatLearningOutcome;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<ListViewModel<RetreatViewModel>> GetRetreats(RetreatFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _retreatRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(r => r.Name.Contains(filter.Name));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status.Value.ToString());
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreats = await paginatedQuery
                .ProjectTo<RetreatViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreats
            };
        }

        public async Task<RetreatViewModel> GetRetreat(Guid id)
        {
            return await _retreatRepository.GetMany(r => r.Id == id)
                .ProjectTo<RetreatViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat not found");
        }

        public async Task<RetreatViewModel> CreateRetreat(Guid accountId, CreateRetreatModel model)
        {
            var result = 0;
            var retreatId = Guid.Empty;

            using(var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    var endDate = GetEndDate(model.StartDate, model.Duration);
                    retreatId = Guid.NewGuid();
                    
                    await UploadRetreatImage(retreatId, model.Images, false);
                    await UploadRetreatDocument(retreatId, model.Documents, false);
                    await AddLearningOutcome(retreatId, model.LearningOutcome, false);
                    var retreat = new Retreat
                    {
                        Id = retreatId,
                        CreatedBy = accountId,
                        Name = model.Name,
                        Cost = model.Cost,
                        Capacity = model.Capacity,
                        Duration = model.Duration,
                        Description = model.Description,
                        StartDate = model.StartDate,
                        EndDate = endDate,
                        Status = RetreatStatus.Active.ToString()
                    };
                    _retreatRepository.Add(retreat);

                    result = await _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            };
            return result > 0 ? await GetRetreat(retreatId) : null!;
        }

        

        public async Task<RetreatViewModel> UpdateRetreat(Guid id, UpdateRetreatModel model)
        {
            var existRetreat = await _retreatRepository.GetMany(r => r.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Retreat not found");

            existRetreat.Name = model.Name ?? existRetreat.Name;
            existRetreat.Cost = model.Cost ?? existRetreat.Cost;
            existRetreat.Capacity = model.Capacity ?? existRetreat.Capacity;
            existRetreat.Description = model.Description ?? existRetreat.Description;

            if (model.Duration.HasValue)
            {
                existRetreat.Duration = model.Duration.Value;
                existRetreat.EndDate = existRetreat.StartDate.AddDays(model.Duration.Value);
            }

            if(model.StartDate.HasValue)
            {
                existRetreat.StartDate = model.StartDate.Value;
                existRetreat.EndDate = GetEndDate(existRetreat.StartDate, existRetreat.Duration);
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                existRetreat.Status = GetRetreatStatus(model.Status);
            }

            if(model.Images != null && model.Images.Count > 0)
            {
                await UploadRetreatImage(id, model.Images, true);
            }

            if(model.Documents != null && model.Documents.Count > 0)
            {
                await UploadRetreatDocument(id, model.Documents, true);
            }

            if(model.LearningOutcome != null && model.LearningOutcome.Count > 0)
            {
                await AddLearningOutcome(id, model.LearningOutcome, true);
            }

            _retreatRepository.Update(existRetreat);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreat(id) : null!;
        }

        private DateOnly GetEndDate(DateOnly startDate, int duration)
        {
            if (startDate < DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7)))
            {
                throw new BadRequestException($"The start date ({startDate}) cannot be less than the current date. Please re-enter.");
            }

            if(duration <= 0)
            {
                throw new BadRequestException($"Please re-enter duration");
            }

            return startDate.AddDays(duration);
        }

        private string GetRetreatStatus(string status)
        {
            if (status != RetreatStatus.Active.ToString() && status != RetreatStatus.InActive.ToString())
            {
                throw new BadRequestException("Invalid status. Please provide either 'Active' or 'InActive'.");
            }

            return status;
        }

        private async Task UploadRetreatImage(Guid retreatId, List<IFormFile> images, bool isUpdate)
        {
            CheckImage(images);
            
            if (isUpdate)
            {
                var listImage = await _retreatFileRepository.GetMany(rt => rt.RetreatId == retreatId && rt.Type == RetreatFileType.Image).ToListAsync();
                foreach (var image in listImage)
                {
                    await _cloudStorageService.DeleteImage(image.Id);
                }
                _retreatFileRepository.RemoveRange(listImage);
            }

            foreach (var image in images)
            {
                var retreatImageId = Guid.NewGuid();
                var url = await _cloudStorageService.UploadImage(retreatImageId, image.ContentType, image.OpenReadStream());
                var retreatImage = new RetreatFile
                {
                    Id = retreatImageId,
                    RetreatId = retreatId,
                    Type = RetreatFileType.Image,
                    Url = url
                };
                _retreatFileRepository.Add(retreatImage);
            }
        }

        private async Task UploadRetreatDocument(Guid retreatId, List<IFormFile> documents, bool isUpdate)
        {

            if (isUpdate)
            {
                var listImage = await _retreatFileRepository.GetMany(rt => rt.RetreatId == retreatId && rt.Type == RetreatFileType.Document).ToListAsync();
                foreach (var image in listImage)
                {
                    await _cloudStorageService.DeleteDocument(image.Id);
                }
                _retreatFileRepository.RemoveRange(listImage);
            }

            foreach (var document in documents)
            {
                var retreatDocumentId = Guid.NewGuid();
                var url = await _cloudStorageService.UploadDocument(retreatDocumentId, document.ContentType, document.OpenReadStream());
                var retreatImage = new RetreatFile
                {
                    Id = retreatDocumentId,
                    RetreatId = retreatId,
                    FileName = document.FileName,
                    Type = RetreatFileType.Document,
                    Url = url
                };
                _retreatFileRepository.Add(retreatImage);
            }
        }

        private async Task AddLearningOutcome(Guid retreatId, List<CreateRetreatLearningOutcomeModel> listModel, bool isUpdate)
        {
            if (isUpdate)
            {
                var flag = await _retreatLearningOutcomeRepository.GetMany(r => r.RetreatId == retreatId).ToListAsync();
                if(flag.Count > 0)
                {
                    _retreatLearningOutcomeRepository.RemoveRange(flag);
                }
            }

            foreach(var model in listModel)
            {
                var retreatLearningOutcome = new RetreatLearningOutcome
                {
                    Id = Guid.NewGuid(),
                    RetreatId= retreatId,
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Description = model.Description,
                };
                _retreatLearningOutcomeRepository.Add(retreatLearningOutcome);
            }

            await Task.CompletedTask;
        }

        private void CheckImage(List<IFormFile> images)
        {
            var imageCount = images.Count();

            if (imageCount < 1 || imageCount > 4)
            {
                throw new BadRequestException("There must be at least one image to create and no more than 4 images");
            }

            foreach (IFormFile image in images)
            {
                if (!image.ContentType.StartsWith("image/"))
                {
                    throw new BadRequestException("Files are not images");
                }
            }
        }
    }
}
