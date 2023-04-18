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
        }
        
    }
}