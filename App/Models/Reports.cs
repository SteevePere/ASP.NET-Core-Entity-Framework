using System;


namespace App.Models
{

    public class Reports
    {

        public int RptId { get; set; }
        public int RptRating { get; set; }
        public string RptComment { get; set; }
        public DateTime RptCreationDatetime { get; set; }
        public DateTime RptEditionDatetime { get; set; }
        public Patients Pat { get; set; }
        public Conditions Cdn { get; set; }
        public Treatments Tmt { get; set; }
    }
}
