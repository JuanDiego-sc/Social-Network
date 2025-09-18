using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain;
namespace Application.Core;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        //!This property should be populated in all of the projections that get the user info
        string? currentUserId = null;
        
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
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.User.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.User.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(
                s => s.User.Followers.Any(x => x.Observer.Id == currentUserId)));


        CreateMap<User, UserProfile>()
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(
                s => s.Followers.Any(x => x.Observer.Id == currentUserId)));


        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));

            
    }
}
