using System;


namespace App.Models
{

    public class Patients
    {

        public int PatId { get; set; }
        public int PtcId { get; set; }
        public int GdrId { get; set; }
        public Genders Gdr { get; set; }
        public string PatFirstName { get; set; }
        public string PatLastName { get; set; }
        public DateTime PatDob { get; set; }
        public short PatHeight { get; set; }
        public short PatWeight { get; set; }
        public short PatIsSmoker { get; set; }
        public short PatIsPregnant { get; set; }
    }
}
