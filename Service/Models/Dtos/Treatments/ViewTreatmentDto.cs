using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Treatments
{

    public class ViewTreatmentDto
    {
        [Required]
        public int TmtId { get; set; }

        [Required]
        public string TmtName { get; set; }

        [Required]
        public string TmtDescription { get; set; }
    }
}
