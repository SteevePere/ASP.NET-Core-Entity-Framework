using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Service.Models
{

    public partial class GendersGdr
    {

        public GendersGdr()
        {
            PatientsPat = new HashSet<PatientsPat>();
            UsersUsr = new HashSet<UsersUsr>();
        }


        public int GdrId { get; set; }
        public string GdrName { get; set; }

        public virtual ICollection<PatientsPat> PatientsPat { get; set; }
        public virtual ICollection<UsersUsr> UsersUsr { get; set; }
    }
}
