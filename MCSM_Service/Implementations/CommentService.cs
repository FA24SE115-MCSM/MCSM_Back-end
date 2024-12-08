using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly INotificationService _notificationService;
        
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService) : base(unitOfWork, mapper)
        {
            _commentRepository = unitOfWork.Comment;
            _postRepository = unitOfWork.Post;
            _notificationService = notificationService;
        }

        public async Task<CommentViewModel> GetComment(Guid id)
        {
            return await _commentRepository.GetMany(c => c.Id == id)
                .ProjectTo<CommentViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Comment not found");
        }

        public async Task<CommentViewModel> CreateComment(Guid accountId, CreateCommentModel model)
        {
            var post = await CheckPost(model.PostId);
            var commentId = Guid.NewGuid();

            var comment = new Comment
            {
                Id = commentId,
                PostId = model.PostId,
                AccountId = accountId,
                Content = model.Content,
            };
            //send noti
            await SendNotification(post);

            _commentRepository.Add(comment);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetComment(commentId) : null!;
        }

        public async Task<CommentViewModel> ReplyComment(Guid accountId, CreateReplyCommentModel model)
        {
            var flag = await CheckComment(model.CommentId);
            var commentId = Guid.NewGuid();

            var comment = new Comment
            {
                Id = commentId,
                PostId = flag.PostId,
                ParentCommentId = model.CommentId,
                AccountId = accountId,
                Content = model.Content,
            };

            //send noti
            await SendNotification(flag);

            _commentRepository.Add(comment);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetComment(commentId) : null!;
        }

        public async Task<CommentViewModel> UpdateComment(Guid id, UpdateCommentModel model)
        {
            var exsitComment = await _commentRepository.GetMany(c => c.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Comment not found");

            exsitComment.Content = model.Content ?? exsitComment.Content;
            exsitComment.UpdateAt = DateTime.UtcNow.AddHours(7);

            _commentRepository.Update(exsitComment);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetComment(id) : null!;
        }

        private async Task<Post> CheckPost(Guid postId)
        {
            return await _postRepository.GetMany(p => p.Id == postId).FirstOrDefaultAsync() ?? throw new NotFoundException("Post not found");
        }
        
        private async Task<Comment> CheckComment(Guid commentId)
        {
            return await _commentRepository.GetMany(p => p.Id == commentId).Include(src => src.Post).FirstOrDefaultAsync() ?? throw new NotFoundException("Comment not found");

        }
        private async Task SendNotification(Post post)
        {
            var message = new CreateNotificationModel
            {
                Title = "Bài viết của bạn có một bình luận mới",
                Body = $"Bài viết của bạn vừa nhận được một lượt tương tác mới. Hãy xem ngay!",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.Now,
                    Type = NotificationType.PostComment,
                    Link = post.Id.ToString()
                }
            };

            await _notificationService.SendNotification(new List<Guid> { post.CreatedBy }, message);
        }

        private async Task SendNotification(Comment comment)
        {
            var message = new CreateNotificationModel
            {
                Title = "Bình luận của bạn có một phản hồi",
                Body = $"Bình luận của bạn có một phản hồi tương tác mới. Hãy xem ngay!",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.Now,
                    Type = NotificationType.PostReplyComment,
                    Link = comment.PostId.ToString()
                }
            };

            await _notificationService.SendNotification(new List<Guid> { comment.AccountId }, message);
        }
    }
}
