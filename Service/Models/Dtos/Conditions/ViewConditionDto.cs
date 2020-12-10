using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Conditions
{

    public class ViewConditionDto
    {
        [Required]
        public int CdnId { get; set; }

        [Required]
        public string CdnName { get; set; }

        [Required]
        public string CdnDescription { get; set; }
    }
}
