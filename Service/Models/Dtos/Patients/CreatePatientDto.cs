using System;
using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Patients
{

    public class CreatePatientDto
    {

        [Required]
        public int PtcId { get; set; }

        [Required]
        public int GdrId { get; set; }

        [Required]
        public string PatFirstName { get; set; }

        [Required]
        public string PatLastName { get; set; }

        [Required]
        public DateTime PatDob { get; set; }

        [Required]
        public short PatHeight { get; set; }

        [Required]
        public short PatWeight { get; set; }

        [Required]
        public short PatIsSmoker { get; set; }

        [Required]
        public short PatIsPregnant { get; set; }
    }
}
