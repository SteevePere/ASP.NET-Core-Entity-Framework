using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Roles
{

    public class ViewRoleDto
    {

        [Required]
        public int RleId { get; set; }

        [Required]
        public string RleName { get; set; }
    }
}
