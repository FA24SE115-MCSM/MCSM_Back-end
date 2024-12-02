using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Implementations
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _commentRepository = unitOfWork.Comment;
            _postRepository = unitOfWork.Post;
        }

        public async Task<CommentViewModel> GetComment(Guid id)
        {
            return await _commentRepository.GetMany(c => c.Id == id)
                .ProjectTo<CommentViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Comment not found");
        }

        public async Task<CommentViewModel> CreateComment(Guid accountId, CreateCommentModel model)
        {
            await CheckPost(model.PostId);
            var commentId = Guid.NewGuid();

            var comment = new Comment
            {
                Id = commentId,
                PostId = model.PostId,
                AccountId = accountId,
                Content = model.Content,
            };

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

        private async Task CheckPost(Guid postId)
        {
            var post = await _postRepository.GetMany(p => p.Id == postId).FirstOrDefaultAsync() ?? throw new NotFoundException("Post not found");

        }
        
        private async Task<Comment> CheckComment(Guid commentId)
        {
            return await _commentRepository.GetMany(p => p.Id == commentId).FirstOrDefaultAsync() ?? throw new NotFoundException("Comment not found");

        }

    }
}
