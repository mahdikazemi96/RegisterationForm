using AutoMapper;
using RegisterationForm.Domain.ResponseDto;
using RegisterationForm.Infrastructure.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.BL.Business
{
    public class PersonalityBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersonalityBusiness(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public string GetPersonalitiesTitle(List<int> ids)
        {
            var titleList= _unitOfWork.PersonalityRepository.GetAll(expression: c => ids.Contains(c.Id))?
                                                            .Select(c => c.Title)?
                                                            .ToList();
            return string.Join(", ", titleList);
        }

        public IEnumerable<PersonalityDto> GetAllPersonalities()
        {
            var allPersonality = _unitOfWork.PersonalityRepository.GetAll();
            var allPersonalityDto = _mapper.Map<List<PersonalityDto>>(allPersonality);
            return allPersonalityDto;
        }
    }
}
