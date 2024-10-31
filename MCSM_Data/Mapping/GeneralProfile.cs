﻿using MCSM_Data.Entities;
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

            CreateMap<RetreatRegistration, RetreatRegistrationViewModel>()
                .ForMember(dest => dest.RetreatName, otp => otp.MapFrom(retreatReg => retreatReg.Retreat.Name));
                //.ForMember(dest => dest.CreateBy, otp => otp.MapFrom(retreatReg => retreatReg.CreateByNavigation))
                //.ForMember(dest => dest.RetreatId, otp => otp.MapFrom(retreatReg => retreatReg.RetreatId))
                //.ForMember(dest => dest.CreateAt, otp => otp.MapFrom(retreatReg => retreatReg.CreateAt))
                //.ForMember(dest => dest.UpdateAt, otp => otp.MapFrom(retreatReg => retreatReg.UpdateAt))
                //.ForMember(dest => dest.TotalCost, otp => otp.MapFrom(retreatReg => retreatReg.TotalCost))
                //.ForMember(dest => dest.TotalParticipants, otp => otp.MapFrom(retreatReg => retreatReg.TotalParticipants))
                //.ForMember(dest => dest.IsDeleted, otp => otp.MapFrom(retreatReg => retreatReg.IsDeleted))
                //.ForMember(dest => dest.IsPaid, otp => otp.MapFrom(retreatReg => retreatReg.IsPaid));
            CreateMap<RetreatRegistrationParticipant, RetreatRegistrationParticipantViewModel>()
                .ForMember(dest => dest.RetreatRegId, otp => otp.MapFrom(retreatPar => retreatPar.RetreatReg.Id))
                .ForMember(dest => dest.PractitionerMail, otp => otp.MapFrom(retreatPar => retreatPar.Participant.Email));

            CreateMap<Lesson, LessonViewModel>()
                .ForMember(dest => dest.CreatorId, otp => otp.MapFrom(lesson => lesson.CreatedByNavigation.Id))
                .ForMember(dest => dest.CreatorFirstName, otp => otp.MapFrom(lesson => lesson.CreatedByNavigation.Profile.FirstName))
                .ForMember(dest => dest.CreatorLastName, otp => otp.MapFrom(lesson => lesson.CreatedByNavigation.Profile.LastName));
            CreateMap<RetreatLesson, RetreatLessonViewModel>()
                .ForMember(dest => dest.LessonTitle, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.Title))
                .ForMember(dest => dest.AuthorId, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.CreatedByNavigation.Id))
                .ForMember(dest => dest.AuthorFirstName, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.CreatedByNavigation.Profile!.FirstName))
                .ForMember(dest => dest.AuthorLastName, otp => otp.MapFrom(retreatLesson => retreatLesson.Lesson.CreatedByNavigation.Profile!.LastName));
            CreateMap<RetreatMonk, RetreatMonkViewModel>()
                .ForMember(dest => dest.MonkFirstName, otp => otp.MapFrom(retreatMonk => retreatMonk.Monk.Profile!.FirstName))
                .ForMember(dest => dest.MonkLastName, otp => otp.MapFrom(retreatMonk => retreatMonk.Monk.Profile!.LastName));
            CreateMap<Tool, ToolViewModel>();
            CreateMap<ToolHistory, ToolHistoryViewModel>();
        }
    }
}
