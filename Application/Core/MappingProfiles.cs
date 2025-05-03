using System;
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
    }
}
