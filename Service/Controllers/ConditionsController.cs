using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Service.Models.Dtos.Conditions;
using Service.Models.Auth;


namespace Service.Controllers
{

    [Route("api/conditions")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {

        private readonly docnetContext _context;


        public ConditionsController(docnetContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<ViewConditionDto>> GetConditions()
        {
            return _context.ConditionsCdn
                .Select(condition => new ViewConditionDto
                {
                    CdnId = condition.CdnId,
                    CdnName = condition.CdnName,
                    CdnDescription = condition.CdnDescription,
                })
                .ToList();
        }


        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ViewConditionDto> GetCondition(int id)
        {
            var condition = _context.ConditionsCdn
                .Select(condition => new ViewConditionDto
                {
                    CdnId = condition.CdnId,
                    CdnName = condition.CdnName,
                    CdnDescription = condition.CdnDescription,
                })
                .Where(condition => condition.CdnId == id)
                .FirstOrDefault();

            if (condition == null)
            {
                return NotFound();
            }

            return condition;
        }


        [HttpPost]
        [Authorize]
        public ActionResult<ConditionsCdn> PostCondition(CreateConditionDto condition)
        {
            ConditionsCdn conditionToCreate = new ConditionsCdn
            {
                CdnName = condition.CdnName,
                CdnDescription = condition.CdnDescription
            };

            _context.ConditionsCdn.Add(conditionToCreate);
            _context.SaveChanges();

            return conditionToCreate;
        }


        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<ConditionsCdn> PutCondition(int id, UpdateConditionDto condition)
        {
            ConditionsCdn conditionToUpdate = _context.ConditionsCdn
                .Where(condition => condition.CdnId == id)
                .FirstOrDefault();

            if (conditionToUpdate == null)
            {
                return NotFound();
            }

            conditionToUpdate.CdnName = condition.CdnName;
            conditionToUpdate.CdnDescription= condition.CdnDescription;

            try
            {
                _context.SaveChanges();
            }

            catch
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return conditionToUpdate;
        }
        

        [HttpDelete("{id}")]
        [Authorize(Roles = RolesModel.Admin)]
        public ActionResult<ConditionsCdn> DeleteCondition(int id)
        {
            var conditionToDelete = _context.ConditionsCdn.Find(id);

            if (conditionToDelete == null)
            {
                return NotFound();
            }

            _context.ConditionsCdn.Remove(conditionToDelete);
            _context.SaveChanges();

            return conditionToDelete;
        }
    }
}
