using System.Collections.Generic;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace App.Controllers
{

    public class PracticesController : Controller
    {

        private HttpService _HttpService { get; }


        public PracticesController(HttpService httpService)
        {
            _HttpService = httpService;
        }
        

        public async Task<IActionResult> ListPractices()
        {
            if (_HttpService.isBearer() != true)
            {
                return View("~/Views/Auth/Login.cshtml");
            }
            string response = await this._HttpService.Get("/api/practices");
            List<Practices> practices = JsonConvert.DeserializeObject<List<Practices>>(response);
            return View(practices);
        }
    }
}