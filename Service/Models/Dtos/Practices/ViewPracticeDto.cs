using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Practices
{

    public class ViewPracticeDto
    {

        [Required]
        public int PtcId { get; set; }

        [Required]
        public string PtcName { get; set; }

        [Required]
        public string PtcAddress { get; set; }

        [Required]
        public string PtcCity { get; set; }

        [Required]
        public string PtcZipCode { get; set; }
    }
}
