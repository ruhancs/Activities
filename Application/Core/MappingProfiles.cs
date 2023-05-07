using Application.Activities;
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
            
            //transforma de ActivityAttendee para profile
            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(p => p.DisplayName, a => a.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(p => p.Username, a => a.MapFrom(s => s.AppUser.UserName))
                .ForMember(p => p.Bio, a => a.MapFrom(s => s.AppUser.Bio));
        }
        
    }
}