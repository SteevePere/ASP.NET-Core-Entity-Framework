using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Reports
{

    public class CreateReportDto
    {

        [Required]
        public int PatId { get; set; }

        [Required]
        public int CdnId { get; set; }

        [Required]
        public int TmtId { get; set; }

        [Required]
        public int RptRating { get; set; }

        public string RptComment { get; set; }
    }
}
