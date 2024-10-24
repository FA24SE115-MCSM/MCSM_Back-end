using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel> GetProfile(Guid id);
        Task<ProfileViewModel> UpdateProfile(Guid id, UpdateProfileModel model);
    }
}
