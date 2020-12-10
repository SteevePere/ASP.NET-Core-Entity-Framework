using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Treatments
{

    public class CreateTreatmentDto
    {

        [Required]
        public string TmtName { get; set; }

        [Required]
        public string TmtDescription { get; set; }
    }
}
