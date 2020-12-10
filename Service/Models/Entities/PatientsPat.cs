using System;
using System.Collections.Generic;


namespace Service.Models
{

    public partial class PatientsPat
    {

        public PatientsPat()
        {
            ReportsRpt = new HashSet<ReportsRpt>();
        }


        public int PatId { get; set; }
        public int PtcId { get; set; }
        public int GdrId { get; set; }
        public string PatFirstName { get; set; }
        public string PatLastName { get; set; }
        public DateTime PatDob { get; set; }
        public short PatHeight { get; set; }
        public short PatWeight { get; set; }
        public short PatIsSmoker { get; set; }
        public short PatIsPregnant { get; set; }

        public virtual GendersGdr Gdr { get; set; }
        public virtual PracticesPtc Ptc { get; set; }
        public virtual ICollection<ReportsRpt> ReportsRpt { get; set; }
    }
}
