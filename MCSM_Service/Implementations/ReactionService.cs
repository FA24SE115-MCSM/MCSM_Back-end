using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
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
    public class ReactionService : BaseService, IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IPostRepository _postRepository;
        public ReactionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _reactionRepository = unitOfWork.Reaction;
            _postRepository = unitOfWork.Post;
        }

        public async Task<ReactionViewModel> GetReaction(Guid id)
        {
            return await _reactionRepository.GetMany(r => r.Id == id)
                .ProjectTo<ReactionViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Reaction not found");
        }


        public async Task<ReactionViewModel> CreateReaction(Guid accountId, CreateReactionModel model)
        {
            await CheckPost(model.PostId);
            var flag = await ValidReaction(accountId, model.PostId);
            if (flag)
            {
                throw new ConflictException("The account has already reaction to this post");
            }

            var reactionId = Guid.NewGuid();
            var reaction = new Reaction
            {
                Id = reactionId,
                PostId = model.PostId,
                AccountId = accountId,
            };
            _reactionRepository.Add(reaction);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetReaction(reactionId) : null!;
        }

        public async Task<bool> UpdateReaction(Guid reactId, Guid accountId)
        {
            //var flag = await ValidReaction(accountId, model.PostId);
            //if (!flag)
            //{
            //    throw new ConflictException("The account not have reaction for this post");
            //}
            var reaction = await _reactionRepository.GetMany(p => p.Id == reactId).FirstOrDefaultAsync() ?? throw new NotFoundException("Reaction not found");
            if(reaction.AccountId != accountId)
            {
                throw new ConflictException("You do not have permission to delete this reaction.");
            }

            _reactionRepository.Remove(reaction);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? true : false;
        }

        private async Task<bool> ValidReaction(Guid accountId, Guid postId)
        {
            return await _reactionRepository.AnyAsync(r => r.PostId == postId
                                                && r.AccountId == accountId);
            
        }

        

        private async Task CheckPost(Guid postId)
        {
            var post = await _postRepository.GetMany(p => p.Id == postId).FirstOrDefaultAsync() ?? throw new NotFoundException("Post not found");

        }
    }
}
