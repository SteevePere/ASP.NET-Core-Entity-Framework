using System.Collections.Generic;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.Models;
using App.Models.ViewModels;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace App.Controllers
{

    public class ReportsController : Controller
    {

        private HttpService _HttpService { get; }


        public ReportsController(HttpService httpService)
        {
            _HttpService = httpService;
        }


        public async Task<IActionResult> ListReports(string nbElement = "5", string nbPage = "1")
        {
            if (_HttpService.isBearer() != true)
            {
                return View("~/Views/Auth/Login.cshtml");
            }

            if (int.Parse(nbElement) < 5)
            {
                nbElement = "5";
            }

            if (int.Parse(nbPage) < 1)
            {
                nbPage = "1";
            }
            
            var response = await _HttpService.GetRequest("/api/reports?PageNumber=" + nbPage + "&PageSize=" + nbElement);
            string apiResponse = await response.Content.ReadAsStringAsync();
            
            List<Reports> reports = JsonConvert.DeserializeObject<List<Reports>>(apiResponse);
            IEnumerable<string> headerValues = response.Headers.GetValues("X-Pagination");
            string headersString = headerValues.First();
            Pagination pagination = JsonConvert.DeserializeObject<Pagination>(headersString);
                        
            ViewBag.pagination = pagination;
            ViewBag.nbElement = nbElement;
            ViewBag.nbPage = nbPage;
            
            return View(reports);
        }


        public async Task<IActionResult> CreateReportAsync()
        {
            string treatmentsString = await _HttpService.Get("/api/treatments");
            List<Treatments> treatments = JsonConvert.DeserializeObject<List<Treatments>>(treatmentsString);

            string conditionsString = await _HttpService.Get("/api/conditions");
            List<Conditions> conditions = JsonConvert.DeserializeObject<List<Conditions>>(conditionsString);

            string patientsString = await _HttpService.Get("/api/patients");
            List<Patients> patients = JsonConvert.DeserializeObject<List<Patients>>(patientsString);

            CreateReportViewModel createReportViewModel = new CreateReportViewModel
            {
                Treatments = treatments,
                Conditions = conditions,
                Patients = patients,
            };

            return View(createReportViewModel);
        }


        public async Task<IActionResult> CreateReportPost()
        {
            ReportToCreate reportToCreate = new ReportToCreate
            {
                PatId = int.Parse(Request.Form["Patients"]),
                CdnId = int.Parse(Request.Form["Conditions"]),
                TmtId = int.Parse(Request.Form["Treatments"]),
                RptRating = int.Parse(Request.Form["Rating"]),
            };

            if (!String.IsNullOrEmpty(Request.Form["Comment"]))
            {
                reportToCreate.RptComment = Request.Form["Comment"];
            }

            var reportToCreateJson = JsonConvert.SerializeObject(reportToCreate);
            await _HttpService.Post("/api/reports", reportToCreateJson);

            return RedirectToAction("ListReports", "Reports");
        }
        
        
        public async Task<IActionResult> UpdateReportPost(int rptId)
        {
            ReportToCreate reportToCreate = new ReportToCreate
            {
                PatId = int.Parse(Request.Form["Patients"]),
                CdnId = int.Parse(Request.Form["Conditions"]),
                TmtId = int.Parse(Request.Form["Treatments"]),
                RptRating = int.Parse(Request.Form["Rating"]),
            };

            if (!String.IsNullOrEmpty(Request.Form["Comment"]))
            {
                reportToCreate.RptComment = Request.Form["Comment"];
            }

            var reportToCreateJson = JsonConvert.SerializeObject(reportToCreate);
            await _HttpService.Put("/api/reports/" + rptId, reportToCreateJson);

            return RedirectToAction("ListReports", "Reports");
        }
        
        
        public async Task<IActionResult> DeleteReport(int rptId)
        {
            await _HttpService.Delete("/api/reports/" + rptId);
            
            return RedirectToAction("ListReports", "Reports");
        }

        
        public async Task<IActionResult> UpdateReport(int rptId)
        {
            string reportString = await _HttpService.Get("/api/reports/" + rptId);
            Reports report = JsonConvert.DeserializeObject<Reports>(reportString);

            string treatmentsString = await _HttpService.Get("/api/treatments");
            List<Treatments> treatments = JsonConvert.DeserializeObject<List<Treatments>>(treatmentsString);

            string conditionsString = await _HttpService.Get("/api/conditions");
            List<Conditions> conditions = JsonConvert.DeserializeObject<List<Conditions>>(conditionsString);

            string patientsString = await _HttpService.Get("/api/patients");
            List<Patients> patients = JsonConvert.DeserializeObject<List<Patients>>(patientsString);

            var PatientsTest = new SelectList(patients, "PatId", "PatLastName").FirstOrDefault(d =>
                d.Value == report.Pat.PatId.ToString());
            
            UpdateReportViewModel updateReportViewModel = new UpdateReportViewModel
            {
                Report = report,
                Treatments = new SelectList(treatments,"TmtId","TmtName", report.Tmt.TmtId.ToString()),
                Conditions = new SelectList(conditions,"CdnId","CdnName", report.Cdn.CdnId.ToString()),
                Patients = new SelectList(patients, "PatId", "PatLastName", report.Pat.PatId),
            };

            return View(updateReportViewModel);
        }
    }
}
