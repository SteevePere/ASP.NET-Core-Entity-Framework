using Service.Models.Dtos.Genders;
using Service.Models.Dtos.Practices;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;



namespace Service.Models.Dtos.Patients
{

    public class ViewPatientDto
    {

        [Required]
        public int PatId { get; set; }

        [Required]
        public int PtcId { get; set; }

        [Required]
        public int GdrId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PatFirstName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewPracticeDto Ptc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewGenderDto Gdr { get; set; }
    }
}
