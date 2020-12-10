using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Genders
{

    public class ViewGenderDto
    {

        [Required]
        public int GdrId { get; set; }

        [Required]
        public string GdrName { get; set; }
    }
}
