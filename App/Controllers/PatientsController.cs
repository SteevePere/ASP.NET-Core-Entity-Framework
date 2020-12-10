using System.Collections.Generic;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.Models;
using App.Models.ViewModels;
using System;


namespace App.Controllers
{

    public class PatientsController : Controller
    {

        private HttpService _HttpService { get; }


        public PatientsController(HttpService httpService)
        {
            _HttpService = httpService;
        }


        public async Task<IActionResult> ListPatients()
        {
            if (_HttpService.isBearer() != true)
            {
                return View("~/Views/Auth/Login.cshtml");
            }
            string response = await this._HttpService.Get("/api/patients");
            List<Patients> patients = JsonConvert.DeserializeObject<List<Patients>>(response);

            return View(patients);
        }


        public async Task<IActionResult> CreatePatientAsync()
        {
            string genderString = await _HttpService.Get("/api/genders");
            List<Genders> genders = JsonConvert.DeserializeObject<List<Genders>>(genderString);

            string practicesString = await _HttpService.Get("/api/practices");
            List<Practices> practices = JsonConvert.DeserializeObject<List<Practices>>(practicesString);
            
            CreatePatientViewModel createPatientViewModel = new CreatePatientViewModel
            {
                Genders = genders,
                Practices = practices
            };

            return View(createPatientViewModel);
        }


        public async Task<IActionResult> CreatePatientPost()
        {
            PatientToCreate patientToCreate = new PatientToCreate
            {
                patHeight = short.Parse(Request.Form["Height"]),
                patWeight = short.Parse(Request.Form["Weight"]),
                patFirstName = Request.Form["Firstname"],
                patLastName = Request.Form["Lastname"],
                patDob = DateTime.Parse(Request.Form["Date"]),
                ptcId = int.Parse(Request.Form["Practice"]),
                gdrId = int.Parse(Request.Form["Gender"]),
            };
            
            if (!String.IsNullOrEmpty(Request.Form["Pregnant"]))
            {
                patientToCreate.patIsPregnant = 1;
            }
            
            if (!String.IsNullOrEmpty(Request.Form["Smoker"]))
            {
                patientToCreate.patIsSmoker = 1;
            }

            var patientToCreateJson = JsonConvert.SerializeObject(patientToCreate);
            var result = await _HttpService.Post("/api/patients", patientToCreateJson);

            return RedirectToAction("ListPatients", "Patients");
        }
    }
}
