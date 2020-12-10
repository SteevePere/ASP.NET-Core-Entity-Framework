using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Users
{

    public class ActivateUserDto
    {

        [Required]
        public short UsrActive { get; set; }
    }
}
