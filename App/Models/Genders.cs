using System.ComponentModel.DataAnnotations;


namespace App.Models
{

    public class Genders
    {

        [Required]
        public int GdrId { get; set; }

        [Required]
        public string GdrName { get; set; }
    }
}
