using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Controllers
{
    public class ConditionsController : Controller
    {
        private HttpService _HttpService;
                    private const string _controllerUrl = "/api/conditions";
         
                    public ConditionsController(HttpService httpService)
                    {
                        _HttpService = httpService;
                    }
        
               public async Task<IActionResult> ConditionsView()
                    {
                        if (_HttpService.isBearer() != true)
                        {
                            return View("~/Views/Auth/Login.cshtml");
                        }
                        string response = await this._HttpService.Get(_controllerUrl);
                        var listConditions = JsonConvert.DeserializeObject<List<ConditionsModel>>(response);
                       
                        return View(listConditions);
                    }

               public async Task<IActionResult> createConditions()
                          {
                               ConditionsModel conditions = new ConditionsModel
                                                                       {
                                       CdnName = Request.Form["CdnName"],
                                       CdnDescription = Request.Form["CdnDescription"]
                                                                  };
                               var objectConditions = JsonConvert.SerializeObject(conditions);
                               var response = await this._HttpService.Post(_controllerUrl, objectConditions);
                               string apiResponse = await response.Content.ReadAsStringAsync();
                               if (response.StatusCode == (HttpStatusCode) 200)
                               {
                                       ViewBag.messageCreate = "Your Conditions has been add successfully";
                                       return RedirectToAction("ConditionsView");
                               }
                               ViewBag.error = "An error has encored";
                               return View("~/Views/Conditions/ConditionsView.cshtml");   
                           }

               public async Task<IActionResult> DeleteConditions(int id)
               {
                   string response = await this._HttpService.Delete(_controllerUrl + '/' + id);
                   return RedirectToAction("ConditionsView");
               }
    }
}