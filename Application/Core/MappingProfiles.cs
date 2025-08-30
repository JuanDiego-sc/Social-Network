using System;
using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>()
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src =>
                src.Date.Kind == DateTimeKind.Utc
                    ? src.Date
                    : DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));

        CreateMap<CreateActivityDto, Activity>()
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src =>
                src.Date.Kind == DateTimeKind.Utc
                    ? src.Date
                    : DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));

        CreateMap<EditActivityDto, Activity>();


        //Relationships mapping

        CreateMap<Activity, ActivityDto>()
            .ForMember(d => d.HostDisplayName,
                o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost)!.User.DisplayName))
            .ForMember(d => d.HostId,
                o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost)!.User.Id));

        CreateMap<ActivityAttendee, UserProfile>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));


        CreateMap<User, UserProfile>();
            
    }
}
