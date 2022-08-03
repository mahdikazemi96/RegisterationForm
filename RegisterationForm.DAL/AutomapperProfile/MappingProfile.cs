using AutoMapper;
using RegisterationForm.Domain.Model;
using RegisterationForm.Domain.RequestDto;
using RegisterationForm.Domain.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.DAL.AutomapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonInfoResponseDto>().ForMember(c => c.personalitiesIds,
                                                                 d => d.MapFrom(map => map.PersonPersonalities.Select(p => p.PersonalityId)));

            CreateMap<EditPersonDto, Person>();
            CreateMap<CreatePersonDto, Person>();
            
            CreateMap<PersonInfoResponseDto, Person>();


            CreateMap<Personality, PersonalityDto>();

        }
    }
}
