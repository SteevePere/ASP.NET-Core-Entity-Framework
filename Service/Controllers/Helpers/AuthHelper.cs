using Service.Models;
using Service.Models.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace Service.Controllers.Helpers
{

    public class AuthHelper
    {

        public bool UserIsAuthorized(ClaimsPrincipal User, UsersUsr user)
        {
            bool userIsAuthorized = false;
            var currentUserId = int.Parse(User.Identity.Name);

            if (user.UsrId == currentUserId || User.IsInRole(RolesModel.Admin))
            {
                userIsAuthorized = true;
            }

            return userIsAuthorized;
        }


        public bool UserIsAuthorizedList(ClaimsPrincipal User, IEnumerable<UsersUsr> users)
        {

            bool userIsAuthorized = false;
            var currentUserId = int.Parse(User.Identity.Name);

            if (users.Any(user => user.UsrId == currentUserId) || User.IsInRole(RolesModel.Admin))
            {
                userIsAuthorized = true;
            }

            return userIsAuthorized;
        }


        public bool UserIsAdmin(ClaimsPrincipal User)
        {
            bool userIsAdmin = false;

            if (User.IsInRole(RolesModel.Admin))
            {
                userIsAdmin = true;
            }

            return userIsAdmin;
        }
    }
}
