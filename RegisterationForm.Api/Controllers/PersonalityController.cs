using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegisterationForm.BL.Business;
using RegisterationForm.Domain.ResponseDto;
using System.Collections.Generic;

namespace RegisterationForm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalityController : ControllerBase
    {
        private readonly PersonalityBusiness _personalityBusiness;
        public PersonalityController(PersonalityBusiness personalityBusiness)
        {
            _personalityBusiness = personalityBusiness;
        }

        [HttpGet]
        public IEnumerable<PersonalityDto> GetAllPersonality()
        {
            return _personalityBusiness.GetAllPersonalities();
        }
    }
}
