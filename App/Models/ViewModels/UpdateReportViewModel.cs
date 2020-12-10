using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace App.Models.ViewModels
{

    public class UpdateReportViewModel
    {
        
        public Reports Report { get; set; }
        public SelectList Treatments { get; set; }
        public SelectList Conditions { get; set; }
        public SelectList Patients { get; set; }
    }
}
