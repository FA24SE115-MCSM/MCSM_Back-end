﻿using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using MCSM_Data.Models.Requests.Put;
using MCSM_Service.Interfaces;

namespace MCSM_Service.Implementations
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAccountRepository _accountRepository;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _feedbackRepository = unitOfWork.Feedback;
            _accountRepository = unitOfWork.Account;
        }

        public async Task<ListViewModel<FeedbackViewModel>> GetFeedbacks(FeedbackFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _feedbackRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.AccountEmail))
            {
                query = query.Where(f => f.CreatedBy.Equals(_accountRepository.GetAll().Where(a => a.Email.Equals(filter.AccountEmail)).FirstOrDefault().Id));
            }
            if (!string.IsNullOrEmpty(filter.RetreatName))
            {
                query = query.Where(f => f.Retreat.Name.Contains(filter.RetreatName));
            }

            if (filter.RetreatRating != 0)
            {
                query = query.Where(r => r.RetreatRating == filter.RetreatRating);
            }

            if (!filter.IsDeleted)
            {
                query = query.Where(r => !r.IsDeleted);
            }
            else
            {
                query = query.Where(r => r.IsDeleted);
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var feedbacks = await paginatedQuery
                .ProjectTo<FeedbackViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<FeedbackViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = feedbacks
            };
        }

        public async Task<FeedbackViewModel> GetFeedback(Guid id)
        {
            return await _feedbackRepository.GetMany(r => r.Id == id)
                .ProjectTo<FeedbackViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Feedback not found");
        }

        public async Task<List<FeedbackViewModel>> GetFeedbackByAccount(Guid accountId)
        {
            return await _feedbackRepository.GetMany(f => f.CreatedBy == accountId && f.IsDeleted == false)
                .ProjectTo<FeedbackViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<List<FeedbackViewModel>> GetFeedbackByRetreat(Guid retreatId)
        {
            return await _feedbackRepository.GetMany(f => f.RetreatId == retreatId && f.IsDeleted == false)
                .ProjectTo<FeedbackViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<FeedbackViewModel> CreateFeedback(Guid accountId, CreateFeedbackModel model)
        {
            var existFeedback = await _feedbackRepository.GetMany(f => f.CreatedBy == accountId && f.RetreatId == model.RetreatId).FirstOrDefaultAsync();
            if (existFeedback != null)
            {
                existFeedback.RetreatRating = model.RetreatRating;
                existFeedback.MonkRating = model.MonkRating;
                existFeedback.RoomRating = model.RoomRating;
                existFeedback.FoodRating = model.FoodRating;
                existFeedback.YourExperience = model.YourExperience;
                existFeedback.Suggestion = model.Suggestion;
                existFeedback.UpdateAt = DateTime.UtcNow;

                _feedbackRepository.Update(existFeedback);
                var result = await _unitOfWork.SaveChanges();
                return result > 0 ? await GetFeedback(existFeedback.Id) : null!;
            }
            else 
            { 
            var feedbackId = Guid.NewGuid();
            var feedback = new Feedback
            {
                Id = feedbackId,
                CreatedBy = accountId,
                RetreatId = model.RetreatId,
                RetreatRating = model.RetreatRating,
                MonkRating = model.MonkRating,
                RoomRating = model.RoomRating,
                FoodRating = model.FoodRating,
                YourExperience = model.YourExperience,
                Suggestion = model.Suggestion,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            //var checkDuplicate = await _feedbackRepository.GetMany(f => f.CreatedBy == accountId && f.RetreatId == model.RetreatId).FirstOrDefaultAsync();
            //if (checkDuplicate != null)
            //{
            //    throw new BadRequestException("You have already submitted a feedback for this retreat");
            //}
                _feedbackRepository.Add(feedback);
                var result = await _unitOfWork.SaveChanges();
                return result > 0 ? await GetFeedback(feedbackId) : null!;
            }

        }

        public async Task<FeedbackViewModel> UpdateFeedback(Guid feedbackId, UpdateFeedbackModel model)
        {
            var existFeedback = await _feedbackRepository.GetMany(r => r.Id == feedbackId).FirstOrDefaultAsync() ?? throw new NotFoundException("Feedback not found.");

            if (model.RetreatRating.HasValue)
            {
                existFeedback.RetreatRating = model.RetreatRating.Value;
            }

            if (model.MonkRating.HasValue)
            {
                existFeedback.MonkRating = model.MonkRating.Value;
            }

            if (model.RoomRating.HasValue)
            {
                existFeedback.RoomRating = model.RoomRating.Value;
            }

            if (model.FoodRating.HasValue)
            {
                existFeedback.FoodRating = model.FoodRating.Value;
            }
            existFeedback.YourExperience = model.YourExperience ?? existFeedback.YourExperience;
            existFeedback.Suggestion = model.Suggestion ?? existFeedback.Suggestion;
            existFeedback.UpdateAt = DateTime.UtcNow;

            _feedbackRepository.Update(existFeedback);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetFeedback(feedbackId) : null!;
        }

        public async Task<FeedbackViewModel> SoftDeleteFeedback(Guid feedbackId)
        {
            var existFeedback = await _feedbackRepository.GetMany(r => r.Id == feedbackId).FirstOrDefaultAsync() ?? throw new NotFoundException("Feedback not found.");
            existFeedback.IsDeleted = true;

            _feedbackRepository.Update(existFeedback);
            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetFeedback(feedbackId) : null!;
        }
    }
}
