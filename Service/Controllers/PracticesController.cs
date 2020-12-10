using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Service.Models.Dtos.Practices;
using Microsoft.EntityFrameworkCore;
using Service.Controllers.Helpers;
using Service.Models.Auth;


namespace Service.Controllers
{

    [Route("api/practices")]
    [ApiController]
    public class PracticesController : ControllerBase
    {

        private readonly docnetContext _context;
        private readonly AuthHelper _authHelper;


        public PracticesController(docnetContext context)
        {
            _context = context;
            _authHelper = new AuthHelper();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ViewPracticeDto>> GetPractices()
        {
            return _context.PracticesPtc
                .Select(practice => new ViewPracticeDto
                {
                    PtcId = practice.PtcId,
                    PtcName = practice.PtcName,
                    PtcAddress = practice.PtcAddress,
                    PtcCity = practice.PtcCity,
                    PtcZipCode = practice.PtcZipCode,
                })
                .ToList();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ViewPracticeDto> GetPractice(int id)
        {
            var practice = _context.PracticesPtc
                .Select(practice => new ViewPracticeDto
                {
                    PtcId = practice.PtcId,
                    PtcName = practice.PtcName,
                    PtcAddress = practice.PtcAddress,
                    PtcCity = practice.PtcCity,
                    PtcZipCode = practice.PtcZipCode,
                })
                .Where(practice => practice.PtcId == id)
                .FirstOrDefault();

            if (practice == null)
            {
                return NotFound();
            }

            return practice;
        }


        [HttpGet("findByName")]
        [AllowAnonymous]
        public ActionResult GetPracticeByName([FromQuery] string ptcName)
        {
            var practice = _context.PracticesPtc
                .Where(practice => practice.PtcName == ptcName)
                .FirstOrDefault();

            if (practice == null)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpPost]
        [Authorize]
        public ActionResult<PracticesPtc> PostPractice(CreatePracticeDto practice)
        {
            PracticesPtc practiceToCreate = new PracticesPtc
            {
                PtcName = practice.PtcName,
                PtcAddress = practice.PtcAddress,
                PtcCity = practice.PtcCity,
                PtcZipCode = practice.PtcZipCode
            };

            _context.PracticesPtc.Add(practiceToCreate);

            try
            {
                _context.SaveChanges();
            }

            catch (DbUpdateException)
            {
                return Conflict(new { message = $"A Practice named '{practice.PtcName}' already exists." });
            }

            return practiceToCreate;
        }


        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<PracticesPtc> PutPractice(int id, UpdatePracticeDto practice)
        {
            PracticesPtc practiceToUpdate = _context.PracticesPtc
                .Include(practice => practice.UsersUsr)
                .Where(practice => practice.PtcId == id)
                .FirstOrDefault();

            if (practiceToUpdate == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAuthorizedList(User, practiceToUpdate.UsersUsr))
            {
                return Forbid();
            }

            practiceToUpdate.PtcName = practice.PtcName;
            practiceToUpdate.PtcAddress = practice.PtcAddress;
            practiceToUpdate.PtcCity = practice.PtcCity;
            practiceToUpdate.PtcZipCode = practice.PtcZipCode;

            try
            {
                _context.SaveChanges();
            }

            catch (DbUpdateException)
            {
                return Conflict(new { message = $"A Practice named '{practice.PtcName}' already exists." });
            }

            return practiceToUpdate;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = RolesModel.Admin)]
        public ActionResult<PracticesPtc> DeletePractice(int id)
        {
            var practiceToDelete = _context.PracticesPtc.Find(id);

            if (practiceToDelete == null)
            {
                return NotFound();
            }

            _context.PracticesPtc.Remove(practiceToDelete);
            _context.SaveChanges();

            return practiceToDelete;
        }
    }
}
