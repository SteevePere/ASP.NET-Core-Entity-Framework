using Service.Models.Dtos.Genders;
using Service.Models.Dtos.Practices;
using Service.Models.Dtos.Roles;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Users
{

    public class ViewUserAuthDto
    {

        [Required]
        public int UsrId { get; set; }

        [Required]
        public int RleId { get; set; }

        [Required]
        public int GdrId { get; set; }

        [Required]
        public int? PtcId { get; set; }

        [Required]
        public string UsrEmail { get; set; }

        [Required]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UsrPassword { get; set; }

        [Required]
        public string UsrFirstName { get; set; }

        [Required]
        public string UsrLastName { get; set; }

        [Required]
        public short UsrActive { get; set; }

        [Required]
        public DateTime UsrCreationDatetime { get; set; }

        [Required]
        public DateTime UsrEditDatetime { get; set; }

        public ViewRoleDto Rle { get; set; }

        public ViewGenderDto Gdr { get; set; }

        public ViewPracticeDto Ptc { get; set; }
    }
}
