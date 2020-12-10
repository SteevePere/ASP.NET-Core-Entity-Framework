using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace App.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpService _HttpService { get; }
        
        public HomeController(ILogger<HomeController> logger, HttpService httpService)
        {
                _logger = logger;
                _HttpService = httpService;
        }



        public async Task<IActionResult> Index()
        {
            if (_HttpService.isBearer() != true)
            {
                return View("~/Views/Auth/Login.cshtml");
            }

            List<DataPoint> totalRecord = await TotalRecord();
            List<DataPoint> chartPatience = await ChartPatience();
            List<DataPoint> chartReports = await ChartReports();
            
            ViewBag.DataPointsTotalRecord = JsonConvert.SerializeObject(totalRecord);
            ViewBag.DataPoints = JsonConvert.SerializeObject(chartPatience);
            ViewBag.DataPointsReports = JsonConvert.SerializeObject(chartReports);

            return View();
        }

        public async Task<List<DataPoint>> TotalRecord()
        {
            string practicesString = await _HttpService.Get("/api/practices");
            List<Practices> practices = JsonConvert.DeserializeObject<List<Practices>>(practicesString);
            
            string treatmentString = await _HttpService.Get("/api/treatments");
            List<TreatmentModel> treatment = JsonConvert.DeserializeObject<List<TreatmentModel>>(treatmentString);
            
            string conditionsString = await _HttpService.Get("/api/conditions");
            List<ConditionsModel> conditions = JsonConvert.DeserializeObject<List<ConditionsModel>>(conditionsString);
            
            string reportString = await _HttpService.Get("/api/reports/");
            List<Reports> report = JsonConvert.DeserializeObject<List<Reports>>(reportString);
            
            string patientString = await _HttpService.Get("/api/patients/");
            List<Patients> patient = JsonConvert.DeserializeObject<List<Patients>>(patientString);

            List<DataPoint> dataPointsTotalRecord = new List<DataPoint>();
 
            dataPointsTotalRecord.Add(new DataPoint("Practices", practices.Sum(item => item.PtcId)));
            dataPointsTotalRecord.Add(new DataPoint("Treatment", treatment.Sum(item => item.TmtId)));
            dataPointsTotalRecord.Add(new DataPoint("Conditions", conditions.Sum(item => item.CdnId)));
            dataPointsTotalRecord.Add(new DataPoint("Reports", report.Sum(item=> item.RptId)));
            dataPointsTotalRecord.Add(new DataPoint("Patients", patient.Sum(item=> item.PtcId)));

            return dataPointsTotalRecord;
        }

        public async Task<List<DataPoint>> ChartPatience()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            string patientString = await _HttpService.Get("/api/patients/");
            List<Patients> patients = JsonConvert.DeserializeObject<List<Patients>>(patientString);
            if (patients.Sum(item => item.GdrId) > 0)
            {
                ViewBag.isPatient = true;
            }
            var isSomker = 0;
            var isPregnant = 0;
            foreach (var patient in patients)
            {
                if (patient.PatIsSmoker == 1)
                {
                    isSomker += 1;
                }

                if (patient.PatIsPregnant == 1)
                {
                    isPregnant += 1;
                }
            }

            dataPoints.Add(new DataPoint("Smoker", isSomker));
            dataPoints.Add(new DataPoint("Pregnant", isPregnant));
            
            return dataPoints;
        }
        
        public async Task<List<DataPoint>> ChartReports()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            string reportsString = await _HttpService.Get("/api/reports/stats");
            List<StatsReports> reports = JsonConvert.DeserializeObject<List<StatsReports>>(reportsString);

            foreach (var report in reports)
            {
                dataPoints.Add(new DataPoint(report.Cdn.CdnName, report.RefConditionsInReports));
            }
            
            return dataPoints;
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
