using Service.Controllers.Helpers;
using Service.Models;
using Service.Models.Auth;
using Service.Models.Dtos.Genders;
using Service.Models.Dtos.Practices;
using Service.Models.Dtos.Roles;
using Service.Models.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Service.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {

        private readonly docnetContext _context;
        private readonly AuthHelper _authHelper;


        public UsersController(
            docnetContext context
        )
        {
            _context = context;
            _authHelper = new AuthHelper();
        }


        [HttpGet]
        [Authorize]
        public IEnumerable<ViewUserDto> GetUsers()
        {
            var query = _context.UsersUsr
                .Select(user => new ViewUserDto
                {
                    UsrId = user.UsrId,
                    RleId = user.RleId,
                    GdrId = user.GdrId,
                    PtcId = user.PtcId,
                    UsrEmail = user.UsrEmail,
                    UsrFirstName = user.UsrFirstName,
                    UsrLastName = user.UsrLastName,
                    UsrActive = user.UsrActive,
                    UsrCreationDatetime = user.UsrCreationDatetime,
                    UsrEditDatetime = user.UsrEditDatetime,
                    Rle = new ViewRoleDto
                    {
                        RleId = user.Rle.RleId,
                        RleName = user.Rle.RleName,
                    },
                    Gdr = new ViewGenderDto
                    {
                        GdrId = user.Gdr.GdrId,
                        GdrName = user.Gdr.GdrName,
                    },
                    Ptc = new ViewPracticeDto
                    {
                        PtcId = user.Ptc.PtcId,
                        PtcName = user.Ptc.PtcName,
                        PtcAddress = user.Ptc.PtcAddress,
                        PtcCity = user.Ptc.PtcCity,
                        PtcZipCode = user.Ptc.PtcZipCode,
                    },
                });

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);
                query = query.Where(user => user.UsrId == currentUserId);
            }

            var users = query.ToList();

            return users;
        }


        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ViewUserDto> GetUser(int id)
        {
            var user = _context.UsersUsr
                .Select(user => new ViewUserDto
                {
                    UsrId = user.UsrId,
                    RleId = user.RleId,
                    GdrId = user.GdrId,
                    PtcId = user.PtcId,
                    UsrEmail = user.UsrEmail,
                    UsrFirstName = user.UsrFirstName,
                    UsrLastName = user.UsrLastName,
                    UsrActive = user.UsrActive,
                    UsrCreationDatetime = user.UsrCreationDatetime,
                    UsrEditDatetime = user.UsrEditDatetime,
                    Rle = new ViewRoleDto
                    {
                        RleId = user.Rle.RleId,
                        RleName = user.Rle.RleName,
                    },
                    Gdr = new ViewGenderDto
                    {
                        GdrId = user.Gdr.GdrId,
                        GdrName = user.Gdr.GdrName,
                    },
                    Ptc = new ViewPracticeDto
                    {
                        PtcId = user.Ptc.PtcId,
                        PtcName = user.Ptc.PtcName,
                        PtcAddress = user.Ptc.PtcAddress,
                        PtcCity = user.Ptc.PtcCity,
                        PtcZipCode = user.Ptc.PtcZipCode,
                    },
                })
                .Where(user => user.UsrId == id)
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);

                if (user.UsrId != currentUserId)
                {
                    return Forbid();
                }
            }

            return user;
        }


        [HttpGet("findByEmail")]
        [AllowAnonymous]
        public ActionResult GetUserByEmail([FromQuery] string usrEmail)
        {
            var user = _context.UsersUsr
                .Where(user => user.UsrEmail == usrEmail)
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult<UsersUsr> PostUser(CreateUserDto user)
        {
           
            UsersUsr userToCreate = new UsersUsr
            {
                RleId = user.RleId,
                PtcId = user.PtcId,
                GdrId = user.GdrId,
                UsrEmail = user.UsrEmail,
                UsrPassword = BCrypt.Net.BCrypt.HashPassword(user.UsrPassword),
                UsrFirstName = user.UsrFirstName,
                UsrLastName = user.UsrLastName,
                UsrActive = 0,
                UsrCreationDatetime = DateTime.Now,
                UsrEditDatetime = DateTime.Now,
            };

            if (user.Ptc != null)
            {
                userToCreate.Ptc = new PracticesPtc
                {
                    PtcName = user.Ptc.PtcName,
                    PtcAddress = user.Ptc.PtcAddress,
                    PtcCity = user.Ptc.PtcCity,
                    PtcZipCode = user.Ptc.PtcZipCode,
                };
            }

            try
            {
                _context.UsersUsr.Add(userToCreate);
                _context.SaveChanges();
                
                return userToCreate;
            }

            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<UsersUsr> PutUser(int id, UpdateUserDto user)
        {
            UsersUsr userToUpdate = _context.UsersUsr
                .Where(user => user.UsrId == id)
                .FirstOrDefault();

            if (userToUpdate == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAuthorized(User, userToUpdate))
            {
                return Forbid();
            }

            userToUpdate.PtcId = user.PtcId;
            userToUpdate.GdrId = user.GdrId;
            userToUpdate.UsrEmail = user.UsrEmail;
            userToUpdate.UsrPassword = BCrypt.Net.BCrypt.HashPassword(user.UsrPassword);
            userToUpdate.UsrFirstName = user.UsrFirstName;
            userToUpdate.UsrLastName = user.UsrLastName;
            userToUpdate.UsrEditDatetime = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }

            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return userToUpdate;
        }


        [HttpPut("activate/{id}")]
        [Authorize(Roles = RolesModel.Admin)]
        public ActionResult<UsersUsr> ActivateUser(int id, ActivateUserDto user)
        {
            UsersUsr userToUpdate = _context.UsersUsr
                .Where(user => user.UsrId == id)
                .FirstOrDefault();

            if (userToUpdate == null)
            {
                return NotFound();
            }

            if (user.UsrActive != 0 && user.UsrActive != 1)
            {
                return BadRequest(new { message = "UsrActive must either be 1 or 0." });
            }

            userToUpdate.UsrActive = user.UsrActive;

            try
            {
                _context.SaveChanges();
            }

            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return userToUpdate;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = RolesModel.Admin)]
        public ActionResult<UsersUsr> DeleteUser(int id)
        {
            var userToDelete = _context.UsersUsr.Find(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            _context.UsersUsr.Remove(userToDelete);
            _context.SaveChanges();

            return userToDelete;
        }
    }
}
