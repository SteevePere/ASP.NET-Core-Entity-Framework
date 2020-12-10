
using System;

namespace App.Models
{

    public class PatientToCreate
    {
        public int patId { get; set; }
        public int ptcId { get; set; }
        public int gdrId { get; set; }
        public string patFirstName { get; set; }
        public string patLastName { get; set; }
        public DateTime patDob { get; set; }
        public short patHeight { get; set; }
        public short patWeight { get; set; }
        public short patIsSmoker { get; set; }
        public short patIsPregnant { get; set; }
    }
}
