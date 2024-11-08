using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using Microsoft.AspNetCore.Http;

namespace MCSM_Service.Interfaces
{
    public interface IAccountService
    {
        Task<AuthViewModel> Authenticated(AuthRequest auth);
        Task<AuthModel> GetAuth(Guid id);

        Task<ListViewModel<AccountViewModel>> GetAccounts(AccountFilterModel filter, PaginationRequestModel pagination);
        Task<AccountViewModel> GetAccount(Guid id);
        Task<AccountViewModel> CreateAccount(CreateAccountModel model);
        Task<AccountViewModel> UpdateAccount(Guid id, UpdateAccountModel model);
        Task<AccountViewModel> UploadAvatar(Guid id, IFormFile image);
        Task VerifyAccount(string token);
        Task ResetPassword(ResetPasswordModel model);
        Task<List<Guid>> CreateNewAccountForRetreatRegistration(List<CreateAccountModel> listModel);
    }
}
