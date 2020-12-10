using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Service.Models.Dtos.Patients;
using Service.Controllers.Helpers;
using Service.Models.Dtos.Genders;
using Service.Models.Dtos.Practices;


namespace Service.Controllers
{

    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {

        private readonly docnetContext _context;
        private readonly AuthHelper _authHelper;


        public PatientsController(docnetContext context)
        {
            _context = context;
            _authHelper = new AuthHelper();
        }


        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<ViewPatientDto>> GetPatients()
        {
            var query = _context.PatientsPat
                .Select(patient => new ViewPatientDto
                {
                    PatId = patient.PatId,
                    PtcId = patient.PtcId,
                    GdrId = patient.GdrId,
                    PatFirstName = patient.PatFirstName,
                    PatLastName = patient.PatLastName,
                    PatDob = patient.PatDob,
                    PatHeight = patient.PatHeight,
                    PatWeight = patient.PatWeight,
                    PatIsSmoker = patient.PatIsSmoker,
                    PatIsPregnant = patient.PatIsPregnant,
                    Gdr = new ViewGenderDto
                    {
                        GdrId = patient.Gdr.GdrId,
                        GdrName = patient.Gdr.GdrName,
                    },
                    Ptc = new ViewPracticeDto
                    {
                        PtcId = patient.Ptc.PtcId,
                        PtcName = patient.Ptc.PtcName,
                        PtcAddress = patient.Ptc.PtcAddress,
                        PtcCity = patient.Ptc.PtcCity,
                        PtcZipCode = patient.Ptc.PtcZipCode,
                    },
                });

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);
                var currentUsersPracticeId = _context.UsersUsr.Find(currentUserId).PtcId;
                query = query.Where(patient => patient.PtcId == currentUsersPracticeId);
            }

            var patients = query.ToList();

            return patients;
        }


        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ViewPatientDto> GetPatient(int id)
        {
            var patient = _context.PatientsPat
                .Select(patient => new ViewPatientDto
                {
                    PatId = patient.PatId,
                    PtcId = patient.PtcId,
                    GdrId = patient.GdrId,
                    PatFirstName = patient.PatFirstName,
                    PatLastName = patient.PatLastName,
                    PatDob = patient.PatDob,
                    PatHeight = patient.PatHeight,
                    PatWeight = patient.PatWeight,
                    PatIsSmoker = patient.PatIsSmoker,
                    PatIsPregnant = patient.PatIsPregnant,
                    Gdr = new ViewGenderDto
                    {
                        GdrId = patient.Gdr.GdrId,
                        GdrName = patient.Gdr.GdrName,
                    },
                    Ptc = new ViewPracticeDto
                    {
                        PtcId = patient.Ptc.PtcId,
                        PtcName = patient.Ptc.PtcName,
                        PtcAddress = patient.Ptc.PtcAddress,
                        PtcCity = patient.Ptc.PtcCity,
                        PtcZipCode = patient.Ptc.PtcZipCode,
                    },
                })
                .Where(patient => patient.PatId == id)
                .FirstOrDefault();

            if (patient == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);
                var currentUsersPracticeId = _context.UsersUsr.Find(currentUserId).PtcId;

                if (patient.PtcId != currentUsersPracticeId)
                {
                    return Forbid();
                }
            }

            return patient;
        }


        [HttpPost]
        [Authorize]
        public ActionResult<PatientsPat> PostPatient(CreatePatientDto patient)
        {
            var practice = _context.PracticesPtc
                .Where(practice => practice.PtcId == patient.PtcId)
                .FirstOrDefault();

            if (practice == null)
            {
                return BadRequest(new { message = "Please make sure that Practice exists." });
            }

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);
                var currentUsersPracticeId = _context.UsersUsr.Find(currentUserId).PtcId;

                if (practice.PtcId != currentUsersPracticeId)
                {
                    return Forbid();
                }
            }

            PatientsPat patientToCreate = new PatientsPat
            {
                PtcId = patient.PtcId,
                GdrId = patient.GdrId,
                PatFirstName = patient.PatFirstName,
                PatLastName = patient.PatLastName,
                PatDob = patient.PatDob,
                PatHeight = patient.PatHeight,
                PatWeight = patient.PatWeight,
                PatIsSmoker = patient.PatIsSmoker,
                PatIsPregnant = patient.PatIsPregnant
            };

            try
            {
                _context.PatientsPat.Add(patientToCreate);
                _context.SaveChanges();
            }

            catch
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return patientToCreate;
        }


        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<PatientsPat> PutPatient(int id, UpdatePatientDto patient)
        {
            PatientsPat patientToUpdate = _context.PatientsPat
                .Include(patient => patient.Ptc.UsersUsr)
                .Where(patient => patient.PatId == id)
                .FirstOrDefault();

            if (patientToUpdate == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAuthorizedList(User, patientToUpdate.Ptc.UsersUsr))
            {
                return Forbid();
            }

            patientToUpdate.PtcId = patient.PtcId;
            patientToUpdate.GdrId = patient.GdrId;
            patientToUpdate.PatFirstName = patient.PatFirstName;
            patientToUpdate.PatLastName = patient.PatLastName;
            patientToUpdate.PatDob = patient.PatDob;
            patientToUpdate.PatHeight = patient.PatHeight;
            patientToUpdate.PatWeight = patient.PatWeight;
            patientToUpdate.PatIsSmoker = patient.PatIsSmoker;
            patientToUpdate.PatIsPregnant = patient.PatIsPregnant;

            try
            {
                _context.SaveChanges();
            }

            catch
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return patientToUpdate;
        }


        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<PatientsPat> DeletePatient(int id)
        {
            var patientToDelete = _context.PatientsPat
                .Include(patient => patient.Ptc.UsersUsr)
                .Where(patient => patient.PatId == id)
                .FirstOrDefault();

            if (patientToDelete == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAuthorizedList(User, patientToDelete.Ptc.UsersUsr))
            {
                return Forbid();
            }

            _context.PatientsPat.Remove(patientToDelete);
            _context.SaveChanges();

            return patientToDelete;
        }
    }
}
