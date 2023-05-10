using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //para atualizar a activity
            CreateMap<Activity, Activity>();
            
            //transformar activity para activitydto
            CreateMap<Activity, ActivityDto>()
                .ForMember(
                    a => a.HostUsername, 
                    o => o.MapFrom(
                        d => d.Attendees.FirstOrDefault(
                            x => x.IsHost
                            ).AppUser.UserName));
            
            //transforma de ActivityAttendee para AttendeeDto
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(p => p.DisplayName, a => a.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(p => p.Username, a => a.MapFrom(s => s.AppUser.UserName))
                .ForMember(p => p.Bio, a => a.MapFrom(s => s.AppUser.Bio))
                .ForMember(
                    d => d.Image, 
                    o => o.MapFrom(
                        s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url
                    ));
        
            //transformar AppUser para Profile
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(
                    d => d.Image, 
                    o => o.MapFrom(
                        s => s.Photos.FirstOrDefault(x => x.IsMain).Url
                    ));
            
            CreateMap<Comment, CommentDto>()
                .ForMember(p => p.DisplayName, a => a.MapFrom(s => s.Author.DisplayName))
                .ForMember(p => p.Username, a => a.MapFrom(s => s.Author.UserName))
                .ForMember(
                    d => d.Image, 
                    o => o.MapFrom(
                        s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url
                    ));
        }
        
    }
}