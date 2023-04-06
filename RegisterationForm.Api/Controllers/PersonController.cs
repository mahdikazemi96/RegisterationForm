using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegisterationForm.BL.Business;
using RegisterationForm.Domain.RequestDto;
using RegisterationForm.Domain.ResponseDto;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegisterationForm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonBusiness _personBusiness;
        public PersonController(PersonBusiness personBusiness)
        {
            _personBusiness = personBusiness;
        }

        [HttpGet]
        public IEnumerable<PersonInfoResponseDto> GetAllPersonInfo()
        {
            return _personBusiness.GetAllPersonInfo();
        }

        [HttpGet("{id}",Name ="GetPerson")]
        public ActionResult<PersonInfoResponseDto> GetPerson(int id)
        {
            if (id == 0)
                return BadRequest();

            return _personBusiness.GetPersonById(id);
        }

        [HttpPut("{id}")]
        public IActionResult PutPerson(int id, EditPersonDto person)
        {
            if (id == 0 || id != person.Id)
                return BadRequest();

            _personBusiness.UpdatePerson(person);

            return NoContent();
        }

        [HttpPost]
        public IActionResult PostPerson(CreatePersonDto person)
        {
            var result = _personBusiness.InsertPerson(person);

            return CreatedAtAction("GetPerson", new { id=result.Id},null);
        }

        [HttpDelete]
        public IActionResult DeletePerson(int id)
        {
            if (id == 0)
                return BadRequest();

            _personBusiness.DeletePerson(id);

            return NoContent();
        }


    }
}
