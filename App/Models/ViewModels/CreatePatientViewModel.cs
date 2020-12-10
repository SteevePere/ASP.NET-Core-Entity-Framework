using System.Collections.Generic;


namespace App.Models.ViewModels
{

    public class CreatePatientViewModel
    {
        public List<Genders> Genders { get; set; }
        public List<Practices> Practices { get; set; }
    }
}
