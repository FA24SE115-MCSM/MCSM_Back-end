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
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IPostImageRepository _postImageRepository;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _postRepository = unitOfWork.Post;
            _cloudStorageService = cloudStorageService;
            _postImageRepository = unitOfWork.PostImage;
        }

        public async Task<ListViewModel<PostViewModel>> GetPosts(PostFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _postRepository.GetAll();

            if (filter.Status.HasValue)
            {
                query = query.Where(p => p.Status == filter.Status.Value.ToString());
            }

            if (!string.IsNullOrEmpty(filter.Content))
            {
                query = query.Where(p => p.Content!.Contains(filter.Content));
            }

            if(filter.AccountId.HasValue)
            {
                query = query.Where(p => p.CreatedBy == filter.AccountId.Value);
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var posts = await paginatedQuery
                .ProjectTo<PostViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<PostViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = posts
            };
        }

        public async Task<PostViewModel> GetPost(Guid id)
        {
            return await _postRepository.GetMany(p => p.Id == id)
                .ProjectTo<PostViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Post not found");
        }

        public async Task<PostViewModel> CreatePost(Guid accountId, CreatePostModel model)
        {
            var postId = Guid.Empty;
            var result = 0;

            using(var transaction = _unitOfWork.Transaction())
            {
                try
                {
                    postId = Guid.NewGuid();
                    if (model.Images != null && model.Images.Count > 0)
                    {
                        await IsValidImage(model.Images);
                        await UploadPostImage(postId, model.Images, false);
                    }

                    var post = new Post
                    {
                        Id = postId,
                        CreatedBy = accountId,
                        Content = model.Content,
                        Status = PostStatus.Active.ToString()
                    };
                    _postRepository.Add(post);
                    result = await _unitOfWork.SaveChanges();
                    
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result > 0 ? await GetPost(postId) : null!;
        }

        public async Task<PostViewModel> UpdatePost(Guid postId, UpdatePostModel model)
        {
            var existPost = await _postRepository.GetMany(p => p.Id == postId).FirstOrDefaultAsync() ?? throw new NotFoundException("Post not found");

            existPost.Content = model.Content ?? existPost.Content;
            existPost.UpdateAt = DateTime.UtcNow.AddHours(7);

            if (model.Status.HasValue)
            {
                existPost.Status = model.Status.Value.ToString();
            }

            if(model.Images != null && model.Images.Count > 0)
            {
                await IsValidImage(model.Images);
                await UploadPostImage(postId, model.Images, true);
            }
            _postRepository.Update(existPost);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetPost(postId) : null!;

        }

        private async Task UploadPostImage(Guid postId, List<IFormFile> images, bool isUpdate)
        {
            if(isUpdate)
            {
                var listImage = await _postImageRepository.GetMany(i => i.PostId == postId).ToListAsync();
                foreach(var image in listImage)
                {
                    await _cloudStorageService.DeleteImage(image.Id);
                }
                _postImageRepository.RemoveRange(listImage);
            }
            foreach (var image in images)
            {
                var id = Guid.NewGuid();
                var url = await _cloudStorageService.UploadImage(id, image.ContentType, image.OpenReadStream());
                var newImage = new PostImage
                {
                    Id = id,
                    PostId = postId,
                    Url = url
                };
                _postImageRepository.Add(newImage);
            }
        }

        private async Task IsValidImage(List<IFormFile> images)
        {
            foreach(var image in images)
            {
                if (!image.ContentType.StartsWith("image/"))
                {
                    throw new BadRequestException("The file is not an image. Please re-enter");
                }
            }

            await Task.CompletedTask;
        }

    }
}
