using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Treatments
{

    public class UpdateTreatmentDto
    {

        [Required]
        public string TmtName { get; set; }

        [Required]
        public string TmtDescription { get; set; }
    }
}
