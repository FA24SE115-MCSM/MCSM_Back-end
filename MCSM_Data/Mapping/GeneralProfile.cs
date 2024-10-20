using MCSM_Data.Entities;
using MCSM_Data.Models.Views;

namespace MCSM_Data.Mapping
{
    public class GeneralProfile : AutoMapper.Profile
    {
        public GeneralProfile()
        {
            CreateMap<Role, RoleViewModel>();

            CreateMap<Account, AccountViewModel>()
                .ForMember(dest => dest.Role, otp => otp.MapFrom(account => account.Role.Name))
                .ForMember(dest => dest.PhoneNumber, otp => otp.MapFrom(account => account.Profile!.PhoneNumber))
                .ForMember(dest => dest.FirstName, otp => otp.MapFrom(account => account.Profile!.FirstName))
                .ForMember(dest => dest.LastName, otp => otp.MapFrom(account => account.Profile!.LastName))
                .ForMember(dest => dest.DateOfBirth, otp => otp.MapFrom(account => account.Profile!.DateOfBirth))
                .ForMember(dest => dest.Gender, otp => otp.MapFrom(account => account.Profile!.Gender))
                .ForMember(dest => dest.Avatar, otp => otp.MapFrom(account => account.Profile!.Avatar));

            CreateMap<RoomType, RoomTypeViewModel>();
            CreateMap<Room, RoomViewModel>();
            CreateMap<Retreat, RetreatViewModel>()
                .ForMember(dest => dest.CreatedBy, otp => otp.MapFrom(retreat => retreat.CreatedByNavigation));
        }
    }
}
