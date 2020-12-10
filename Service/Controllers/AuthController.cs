using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Service.Models;
using Service.Models.Dtos.Users;
using Service.Models.Dtos.Roles;
using Service.Models.Dtos.Genders;
using Service.Models.Dtos.Practices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Service.Controllers
{

    [ApiController]
    [Route("api/authenticate")]
    public class AuthController : Controller
    {

        private IConfiguration _config;
        private readonly docnetContext _context;


        public AuthController(IConfiguration config, docnetContext context)
        {
            _config = config;
            _context = context;
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(AuthModel loginInfo)
        {
            IActionResult response = Unauthorized();

            var user = AuthenticateUser(loginInfo);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(
                    new
                    {
                        bearer = tokenString,
                        user,
                    }
                );
            }

            return response;
        }


        private string GenerateJSONWebToken(ViewUserAuthDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.UsrId.ToString()),
                new Claim(ClaimTypes.Role, user.Rle.RleName.ToString()),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiresIn"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private ViewUserAuthDto AuthenticateUser(AuthModel loginInfo)
        {
            ViewUserAuthDto authentifiedUser = null;

            if (loginInfo.Login != null && loginInfo.Password != null)
            {
                var userFound = _context.UsersUsr
                    .Select(user => new ViewUserAuthDto
                    {
                        UsrId = user.UsrId,
                        RleId = user.RleId,
                        GdrId = user.GdrId,
                        PtcId = user.PtcId,
                        UsrEmail = user.UsrEmail,
                        UsrPassword = user.UsrPassword,
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
                    .Where(user => user.UsrEmail == loginInfo.Login)
                    .FirstOrDefault();

                if (userFound != null 
                    && userFound.UsrActive == 1
                    && BCrypt.Net.BCrypt.Verify(loginInfo.Password, userFound.UsrPassword))
                {
                    userFound.UsrPassword = null;
                    authentifiedUser = userFound;
                }
            }

            return authentifiedUser;
        }
    }
}
