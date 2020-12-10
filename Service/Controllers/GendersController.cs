using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Service.Models.Dtos.Genders;


namespace Service.Controllers
{

    [Route("api/genders")]
    [ApiController]
    public class GendersController : ControllerBase
    {

        private readonly docnetContext _context;


        public GendersController(
            docnetContext context
        )
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<ViewGenderDto> GetGenders()
        {
            return _context.GendersGdr
                .Select(gender => new ViewGenderDto
                {
                    GdrId = gender.GdrId,
                    GdrName = gender.GdrName,
                })
                .ToList();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ViewGenderDto> GetGender(int id)
        {
            var genderFound = _context.GendersGdr
                .Select(gender => new ViewGenderDto
                {
                    GdrId = gender.GdrId,
                    GdrName = gender.GdrName,
                })
                .Where(gender => gender.GdrId == id)
                .FirstOrDefault();

            if (genderFound == null)
            {
                return NotFound();
            }

            return genderFound;
        }
    }
}
