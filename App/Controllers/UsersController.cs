using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;


namespace App.Controllers
{

    public class UsersController : Controller
    {

        private HttpService _HttpService { get; }
        private const string _controllerUrl = "/api/users/";
        private readonly IMemoryCache memoryCache;


        public UsersController(HttpService httpService, IMemoryCache memoryCache)
        { 
            _HttpService = httpService;
            this.memoryCache = memoryCache;
        }
        

        public async Task<IActionResult> ListUsers()
        {
            if (_HttpService.isBearer() != true)
            {
               return View("~/Views/Auth/Login.cshtml");
            }
            var user = memoryCache.Get<JWT>("CacheUser").user;
            if (user.GdrId != 1)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            List<Users> userList = new List<Users>();

            string response = await this._HttpService.Get("/api/users");
            userList = JsonConvert.DeserializeObject<List<Users>>(response);
            return View(userList);
        }


        public async Task<ActionResult> ActivateUser(int userId)
        {
            string response = await this._HttpService.Get("api/users/" + userId);
            Users currentUser = JsonConvert.DeserializeObject<Users>(response);

            if (currentUser.UsrActive == 0)
            {
                var test = await this._HttpService.Put("/api/users/activate/" + userId, "{\"usrActive\": 1}");
            }
            else
            {
                await this._HttpService.Put("/api/users/activate/" + userId, "{\"usrActive\": 0}");
            }

            return RedirectToAction("ListUsers");
        }
        
        public async Task<IActionResult> GetProfile()
        {
            if (_HttpService.isBearer() != true)
            {
                return View("~/Views/Auth/Login.cshtml");
            }
            Users userCache = memoryCache.Get<JWT>("CacheUser").user;
            string response = await this._HttpService.Get(_controllerUrl + userCache.UsrId);
            userCache = JsonConvert.DeserializeObject<Users>(response);
            if (TempData["messageProfile"] != null)
            {
                ViewBag.messageProfile = TempData["messageProfile"].ToString();
            }

            return View(userCache);
        }
        
        
        public async Task<IActionResult> updateUser()
        {
            Users user = memoryCache.Get<JWT>("CacheUser").user;

            var response = await this._HttpService.Get(_controllerUrl + user.UsrId);
            user = JsonConvert.DeserializeObject<Users>(response);
            
            string usrFirstName = Request.Form["UsrFirstName"];
            if (string.IsNullOrEmpty(usrFirstName))
            {
                usrFirstName = user.UsrFirstName;
            }
            string usrLastName = Request.Form["UsrLastName"];
            if (string.IsNullOrEmpty(usrLastName))
            {
                usrLastName = user.UsrLastName;
            }
            
            string usrPassword = Request.Form["UsrPassword"];
            string reUserPassword = Request.Form["reUsrPassword"];
            if (string.IsNullOrEmpty(usrPassword))
            {
                TempData["messageProfile"] = "Password was empty";
            }

            if (reUserPassword != usrPassword)
            {
                TempData["messageProfile"] = "Password was not same";
                return RedirectToAction("GetProfile");
            }
            

            Users userToUpdate = new Users()
            {
                UsrEmail = user.UsrEmail,
                UsrFirstName = usrFirstName,
                UsrLastName = usrLastName,
                UsrPassword = usrPassword,
                PtcId = user.PtcId,
                GdrId = user.GdrId
            };
            
            
            var objectUserToUpdate = JsonConvert.SerializeObject(userToUpdate);
            var responseUpdate = await this._HttpService.Put(_controllerUrl + user.UsrId, objectUserToUpdate);
            TempData["messageProfile"] = "Your profile has been update";
            return RedirectToAction("GetProfile");

        }

    }
}