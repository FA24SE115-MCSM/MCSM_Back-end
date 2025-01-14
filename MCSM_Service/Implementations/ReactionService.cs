﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class ReactionService : BaseService, IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IPostRepository _postRepository;
        private readonly INotificationService _notificationService;
        public ReactionService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService) : base(unitOfWork, mapper)
        {
            _reactionRepository = unitOfWork.Reaction;
            _postRepository = unitOfWork.Post;
            _notificationService = notificationService;
        }

        public async Task<ReactionViewModel> GetReaction(Guid id)
        {
            return await _reactionRepository.GetMany(r => r.Id == id)
                .ProjectTo<ReactionViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Reaction not found");
        }


        public async Task<ReactionViewModel> CreateReaction(Guid accountId, CreateReactionModel model)
        {
            var post = await CheckPost(model.PostId);
            var existReaction = await ValidReaction(accountId, model.PostId);
            if (existReaction != null)
            {
                _reactionRepository.Remove(existReaction);
                await _unitOfWork.SaveChanges();
                return _mapper.Map<ReactionViewModel>(existReaction);
            }

            var reactionId = Guid.NewGuid();
            var reaction = new Reaction
            {
                Id = reactionId,
                PostId = model.PostId,
                AccountId = accountId,
            };

            await SendNotification(post);

            _reactionRepository.Add(reaction);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetReaction(reactionId) : null!;
        }

        private async Task SendNotification(Post post)
        {
            var message = new CreateNotificationModel
            {
                Title = "Chúc mừng! Bài viết của bạn được yêu thích 🎉",
                Body = $"Bài viết của bạn vừa nhận được một lượt tương tác mới. Hãy xem ngay!",
                Data = new NotificationDataViewModel
                {
                    CreateAt = DateTime.Now,
                    Type = NotificationType.PostReaction,
                    Link = post.Id.ToString()
                }
            };

            await _notificationService.SendNotification(new List<Guid> { post.CreatedBy }, message);
        }


        //public async Task<bool> UpdateReaction(Guid reactId, Guid accountId)
        //{
        //    //var flag = await ValidReaction(accountId, model.PostId);
        //    //if (!flag)
        //    //{
        //    //    throw new ConflictException("The account not have reaction for this post");
        //    //}
        //    var reaction = await _reactionRepository.GetMany(p => p.Id == reactId).FirstOrDefaultAsync() ?? throw new NotFoundException("Reaction not found");
        //    if(reaction.AccountId != accountId)
        //    {
        //        throw new ConflictException("You do not have permission to delete this reaction.");
        //    }

        //    _reactionRepository.Remove(reaction);
        //    var result = await _unitOfWork.SaveChanges();
        //    return result > 0 ? true : false;
        //}

        private async Task<Reaction?> ValidReaction(Guid accountId, Guid postId)
        {
            return await _reactionRepository.GetMany(r => r.PostId == postId && r.AccountId == accountId).FirstOrDefaultAsync();
        }

        

        private async Task<Post> CheckPost(Guid postId)
        {
            return await _postRepository.GetMany(p => p.Id == postId).FirstOrDefaultAsync() ?? throw new NotFoundException("Post not found");
        }
    }
}
