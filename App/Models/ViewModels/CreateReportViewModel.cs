using System.Collections.Generic;


namespace App.Models.ViewModels
{

    public class CreateReportViewModel
    {

        public List<Treatments> Treatments { get; set; }
        public List<Conditions> Conditions { get; set; }
        public List<Patients> Patients { get; set; }
    }
}
