using RegisterationForm.Domain.Model;
using RegisterationForm.Infrastructure.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.BL.Business
{
    public class PersonPersonalityBusiness
    {
        private IUnitOfWork _unitOfWork;

        public PersonPersonalityBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<PersonPersonality> GetByPersonId(int personId)
        {
            return _unitOfWork.PersonPersonalityRepository.GetAll(expression: c => c.PersonId == personId)?.ToList();
        }
        public void DeleteRange(int personId)
        {
            var personPersonalities = GetByPersonId(personId);
            if (personPersonalities.Count > 0)
            {
                _unitOfWork.PersonPersonalityRepository.DeleteRange(personPersonalities);
                _unitOfWork.Save();
            }
        }
        public void InsertRange(int personId, List<int> personalityIds)
        {
            if (personalityIds.Count < 1) return;

            foreach (var personality in personalityIds)
            {
                _unitOfWork.PersonPersonalityRepository.Insert(new PersonPersonality()
                                                               {
                                                                   PersonId = personId,
                                                                   PersonalityId = personality
                                                               });
                _unitOfWork.Save();
            }
        }
    }
}
