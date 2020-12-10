using System.ComponentModel.DataAnnotations;


namespace Service.Models.Dtos.Conditions
{

    public class CreateConditionDto
    {

        [Required]
        public string CdnName { get; set; }

        [Required]
        public string CdnDescription { get; set; }
    }
}
