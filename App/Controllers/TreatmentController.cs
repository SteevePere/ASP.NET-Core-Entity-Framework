using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Controllers
{
    

    public class TreatmentController : Controller
    {
        private HttpService _HttpService;
        private const string _controllerUrl = "/api/treatments";
           
        public TreatmentController(HttpService httpService)
        {
            _HttpService = httpService;
        }
        // GET
        public async Task<IActionResult> TreatmentView()
        {
            if (_HttpService.isBearer() != true)
            {
                return View("~/Views/Auth/Login.cshtml");
            }
            string response = await this._HttpService.Get(_controllerUrl);
            var listTreatment = JsonConvert.DeserializeObject<List<TreatmentModel>>(response);

            ViewBag.userTreatment = listTreatment;
            return View(listTreatment);
        }
        
        public async Task<IActionResult> createTreatment()
        { 
            TreatmentModel treatment = new TreatmentModel
                {
                    TmtName = Request.Form["TmtName"],
                    TmtDescription = Request.Form["TmtDescription"]
                };
                        var objectTreatment = JsonConvert.SerializeObject(treatment);
                        var response = await this._HttpService.Post(_controllerUrl, objectTreatment);
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == (HttpStatusCode) 200)
                        {
                                ViewBag.messageCreate = "Your treatment has been add successfully";
                                return RedirectToAction("TreatmentView");
                        }
                        ViewBag.error = "An error has encored";
                      return View("~/Views/Treatment/TreatmentView.cshtml");
        }
        
        public async Task<IActionResult> DeleteTreatments(int id)
        {
            string response = await this._HttpService.Delete(_controllerUrl + '/' + id);
            return RedirectToAction("TreatmentView");
        }

        public async Task<IActionResult> updateTreatment(int id)
        {
            string response = await this._HttpService.Get(_controllerUrl + id);
            var treatment = JsonConvert.DeserializeObject<TreatmentModel>(response);
            string tmtName = Request.Form["TmtName"];
            if (string.IsNullOrEmpty(tmtName))
            {
                tmtName = treatment.TmtName;
            }
            string tmtDescription = Request.Form["TmtDescription"];
            if (string.IsNullOrEmpty(tmtDescription))
            {
                tmtDescription = treatment.TmtDescription;
            }
            
            TreatmentModel treatmentUpdate = new TreatmentModel
            {
                TmtName = tmtName,
                TmtDescription = tmtDescription
            };
            
            var objectTreatmentToUpdate = JsonConvert.SerializeObject(treatmentUpdate);
            var responseUpdate = await this._HttpService.Put(_controllerUrl + id, objectTreatmentToUpdate);
            TempData["messageProfile"] = "Your profile has been update";
            return RedirectToAction("TreatmentView");
        }


    }
}