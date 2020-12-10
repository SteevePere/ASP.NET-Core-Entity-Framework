using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Service.Models.Dtos.Conditions;


namespace Service.Models.Dtos.Reports
{

    public class StatReportDto
    {

        [Required]
        public int RefConditionsInReports { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewConditionDto Cdn { get; set; }
    }
}
