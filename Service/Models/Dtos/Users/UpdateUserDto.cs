using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Users
{

    public class UpdateUserDto
    {

        public int? PtcId { get; set; }

        [Required]
        public int GdrId { get; set; }

        [Required]
        public string UsrEmail { get; set; }

        [Required]
        public string UsrPassword { get; set; }

        [Required]
        public string UsrFirstName { get; set; }

        [Required]
        public string UsrLastName { get; set; }
    }
}
