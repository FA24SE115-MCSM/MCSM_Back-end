﻿using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Interfaces
{
    public interface IRetreatRegistrationParticipantService
    {
        Task<ListViewModel<RetreatRegistrationParticipantViewModel>> GetRetreatRegistrationParticipants(RetreatRegistrationParticipantFilterModel filter,  PaginationRequestModel pagination);
        Task<RetreatRegistrationParticipantViewModel> GetRetreatRegistrationParticipant(Guid id);
        Task<RetreatRegistrationViewModel> CreateRetreatRegistrationParticipants(CreateRetreatRegistrationParticipantModel model);
        Task<ListViewModel<RetreatRegistrationParticipantViewModel>> GetRetreatRegistratingParticipants(List<Guid> id);
    }
}
