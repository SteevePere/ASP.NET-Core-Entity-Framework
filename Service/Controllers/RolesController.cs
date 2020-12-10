using Service.Models;
using Service.Models.Dtos.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace Service.Controllers
{

    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly docnetContext _context;


        public RolesController(
            docnetContext context
        )
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<ViewRoleDto> GetRoles()
        {
            return _context.RolesRle
                .Select(role => new ViewRoleDto
                {
                    RleId = role.RleId,
                    RleName = role.RleName,
                })
                .ToList();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ViewRoleDto> GetRole(int id)
        {
            var roleFound = _context.RolesRle
                .Select(role => new ViewRoleDto
                {
                    RleId = role.RleId,
                    RleName = role.RleName,
                })
                .Where(role => role.RleId == id)
                .FirstOrDefault();

            if (roleFound == null)
            {
                return NotFound();
            }

            return roleFound;
        }
    }
}
