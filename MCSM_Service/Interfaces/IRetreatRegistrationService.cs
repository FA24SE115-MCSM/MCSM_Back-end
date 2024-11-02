using MCSM_Data.Models.Requests.Filters;
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
    public interface IRetreatRegistrationService
    {
        Task<ListViewModel<RetreatRegistrationViewModel>> GetRetreatRegistrations(RetreatRegistrationFilterModel filter, PaginationRequestModel pagination);
        Task<RetreatRegistrationViewModel> GetRetreatRegistration(Guid id);
        Task<RetreatRegistrationViewModel> CreateRetreatRegistration(CreateRetreatRegistrationModel model);
        Task<ListViewModel<ActiveRetreatRegistrationViewModel>> GetActiveRetreatRegistrationForUser(Guid id, PaginationRequestModel pagination);
    }
}
