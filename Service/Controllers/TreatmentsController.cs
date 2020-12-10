using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Service.Models.Dtos.Treatments;
using Service.Models.Auth;


namespace Service.Controllers
{

    [Route("api/treatments")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {

        private readonly docnetContext _context;


        public TreatmentsController(docnetContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<ViewTreatmentDto>> GetTreatments()
        {
            return _context.TreatmentsTmt
                .Select(treatment => new ViewTreatmentDto
                {
                    TmtId = treatment.TmtId,
                    TmtName = treatment.TmtName,
                    TmtDescription = treatment.TmtDescription,
                })
                .ToList();
        }


        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ViewTreatmentDto> GetTreatment(int id)
        {
            var treatment = _context.TreatmentsTmt
                .Select(treatment => new ViewTreatmentDto
                {
                    TmtId = treatment.TmtId,
                    TmtName = treatment.TmtName,
                    TmtDescription = treatment.TmtDescription,
                })
                .Where(treatment => treatment.TmtId == id)
                .FirstOrDefault();

            if (treatment == null)
            {
                return NotFound();
            }

            return treatment;
        }


        [HttpPost]
        [Authorize]
        public ActionResult<TreatmentsTmt> PostTreatment(CreateTreatmentDto treatment)
        {
            TreatmentsTmt treatmentToCreate = new TreatmentsTmt
            {
                TmtName = treatment.TmtName,
                TmtDescription = treatment.TmtDescription
            };

            _context.TreatmentsTmt.Add(treatmentToCreate);
            _context.SaveChanges();

            return treatmentToCreate;
        }


        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<TreatmentsTmt> PutTreatment(int id, UpdateTreatmentDto treatment)
        {
            TreatmentsTmt treatmentToUpdate = _context.TreatmentsTmt
                .Where(treatment => treatment.TmtId == id)
                .FirstOrDefault();

            if (treatmentToUpdate == null)
            {
                return NotFound();
            }

            treatmentToUpdate.TmtName = treatment.TmtName;
            treatmentToUpdate.TmtDescription= treatment.TmtDescription;

            try
            {
                _context.SaveChanges();
            }

            catch
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return treatmentToUpdate;
        }
        
        
        [HttpDelete("{id}")]
        [Authorize(Roles = RolesModel.Admin)]
        public ActionResult<TreatmentsTmt> DeleteTreatment(int id)
        {
            var treatmentToDelete = _context.TreatmentsTmt.Find(id);

            if (treatmentToDelete == null)
            {
                return NotFound();
            }

            _context.TreatmentsTmt.Remove(treatmentToDelete);
            _context.SaveChanges();

            return treatmentToDelete;
        }
    }
}
