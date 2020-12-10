using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Service.Models.Dtos.Conditions;
using Service.Models.Dtos.Patients;
using Service.Models.Dtos.Treatments;


namespace Service.Models.Dtos.Reports
{

    public class ViewReportDto
    {

        [Required]
        public int RptId { get; set; }

        [Required]
        public int RptRating { get; set; }

        public string RptComment { get; set; }

        [Required]
        public DateTime RptCreationDatetime { get; set; }

        [Required]
        public DateTime RptEditionDatetime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewPatientDto Pat { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewConditionDto Cdn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewTreatmentDto Tmt { get; set; }
    }
}
