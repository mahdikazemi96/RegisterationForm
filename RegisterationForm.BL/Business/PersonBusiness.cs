using RegisterationForm.Domain.ResponseDto;
using RegisterationForm.Infrastructure.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RegisterationForm.Domain.RequestDto;
using RegisterationForm.Domain.Model;

namespace RegisterationForm.BL.Business
{
    public class PersonBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PersonalityBusiness _personalityBusiness;
        private readonly PersonPersonalityBusiness _personPersonalityBusiness;
        public PersonBusiness(IUnitOfWork unitOfWork,
                              IMapper mapper,
                              PersonalityBusiness personalityBusiness,
                              PersonPersonalityBusiness personPersonalityBusiness)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _personalityBusiness = personalityBusiness;
            _personPersonalityBusiness = personPersonalityBusiness;
        }

        public IEnumerable<PersonInfoResponseDto> GetAllPersonInfo()
        {
            var allPersonInfo = _unitOfWork.PersonRepository.GetAll(include: c => c.Include(d => d.PersonPersonalities));

            var allPersonInfoDto = _mapper.Map<List<PersonInfoResponseDto>>(allPersonInfo);

            foreach (var person in allPersonInfoDto)
            {
                person.personalities = _personalityBusiness.GetPersonalitiesTitle(person.personalitiesIds);
            }

            return allPersonInfoDto;
        }

        public PersonInfoResponseDto GetPersonById(int id)
        {
            var person = _unitOfWork.PersonRepository.Get(c => c.Id == id);
            var personDto = _mapper.Map<PersonInfoResponseDto>(person);
            return personDto;
        }

        public Person InsertPerson(CreatePersonDto personDto)
        {
            var person = _mapper.Map<Person>(personDto);
            _unitOfWork.PersonRepository.Insert(person);
            _unitOfWork.Save();

            _personPersonalityBusiness.InsertRange(person.Id, personDto.personalitiesIds);

            return person;
        }

        public bool UpdatePerson(EditPersonDto person)
        {
            try
            {
                var newPerson = _mapper.Map<Person>(person);
                _unitOfWork.PersonRepository.Update(newPerson);
                _unitOfWork.Save();

                _personPersonalityBusiness.DeleteRange(person.Id);
                _personPersonalityBusiness.InsertRange(person.Id, person.personalitiesIds);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeletePerson(int id)
        {
            var person = GetPersonById(id);
            if (person != null)
            {
                _personPersonalityBusiness.DeleteRange(person.Id);
                _unitOfWork.PersonRepository.Delete(id);
                _unitOfWork.Save();
            }
        }
    }
}
