using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Models;
using App.Models.Ajax;
using App.Models.ViewModels;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;


namespace App.Controllers
{

    public class AuthController : Controller
    {

        private HttpService _HttpService;
        private readonly IMemoryCache memoryCache;
        private const string _controllerUrl = "/api/authenticate";


        public AuthController(HttpService httpService, IMemoryCache memoryCache)
        {
            _HttpService = httpService;
            this.memoryCache = memoryCache;
        }

        
        public ViewResult Login()
        {
            return View();
        }


        public ViewResult Logout()
        {
            HttpContext.Session.Clear();
            return View("~/Views/Auth/Login.cshtml");
        }
        
        public async Task<IActionResult> OnPost()
        {
            AuthModel userLogin = new AuthModel
            {
                login = Request.Form["Login"],
                password = Request.Form["Password"]
            };
            var objectUserLogin = JsonConvert.SerializeObject(userLogin);
            using var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true
            };
            try
            {
                var response = await this._HttpService.Post(_controllerUrl, objectUserLogin);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    JWT jwt = JsonConvert.DeserializeObject<JWT>(apiResponse);
                    memoryCache.Set("CacheUser", jwt);  
                    HttpContext.Session.SetString("accessToken", jwt.bearer);
                    return RedirectToAction("Index", "Home");
                
                }
                ViewBag.error = "Invalid credentials. Is your account pending validation?";
                return View("~/Views/Auth/Login.cshtml");
            }
            catch (Exception e)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            
        }


        public async Task<IActionResult> Register()
        {
            string gendersString = await _HttpService.Get("/api/genders");
            List<Genders> genders = JsonConvert.DeserializeObject<List<Genders>>(gendersString);

            string practicesString = await _HttpService.Get("/api/practices");
            List<Practices> practices = JsonConvert.DeserializeObject<List<Practices>>(practicesString);

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Genders = genders,
                Practices = practices
            };

            return View(registerViewModel);
        }


        public async Task<IActionResult> RegisterPost()
        {
            var creatingPractice = Request.Form["CreatingPractice"] == "1";

            UserToCreate userToCreate = new UserToCreate
            {
                GdrId = int.Parse(Request.Form["Genders"]),
                UsrEmail = Request.Form["Email"],
                UsrPassword = Request.Form["Password"],
                UsrFirstName = Request.Form["FirstName"],
                UsrLastName = Request.Form["LastName"],
            };

            if (Request.Form["Role"] == RolesModel.Admin)
            {
                userToCreate.RleId = 1;
            }

            else
            {
                userToCreate.RleId = 2;

                if (!creatingPractice)
                {
                    userToCreate.PtcId = int.Parse(Request.Form["Practices"]);
                }

                else
                {
                    userToCreate.Ptc = new PracticeToCreate
                    {
                        PtcName = Request.Form["PracticeName"],
                        PtcAddress = Request.Form["PracticeAddress"],
                        PtcCity = Request.Form["PracticeCity"],
                        PtcZipCode = Request.Form["PracticeZipCode"],
                    };
                }
            }

            var userToCreateJson = JsonConvert.SerializeObject(userToCreate);
            var response = await _HttpService.Post("/api/users", userToCreateJson);

            if (response.StatusCode == (HttpStatusCode)200)
            {
                ViewBag.success = "Your account has been created successfully. You will be able to sign in as soon as it has been validated.";

                return View("~/Views/Auth/Login.cshtml");
            }

            return new EmptyResult();
        }


        [HttpPost]
        public async Task<HttpStatusCode> Ajax_ValidateEmail(ValidateEmail data)
        {
            HttpStatusCode responseCode = (HttpStatusCode)200;

            try
            {
                await _HttpService.Get("/api/users/findByEmail?usrEmail=" + data.email);
            }

            catch (HttpRequestException)
            {
                responseCode = (HttpStatusCode)404;
            }

            return responseCode;
        }


        [HttpPost]
        public async Task<HttpStatusCode> Ajax_ValidatePractice(ValidatePractice data)
        {
            HttpStatusCode responseCode = (HttpStatusCode)200;

            try
            {
                await _HttpService.Get("/api/practices/findByName?ptcName=" + data.practiceName);
            }

            catch (HttpRequestException) 
            {
                responseCode = (HttpStatusCode)404;
            }

            return responseCode;
        }
    }
}
