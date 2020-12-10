using System.ComponentModel.DataAnnotations;


namespace Service.Models
{

    public partial class AuthModel
    {

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
