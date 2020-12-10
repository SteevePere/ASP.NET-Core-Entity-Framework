using System.Collections.Generic;


namespace Service.Models
{

    public partial class PracticesPtc
    {

        public PracticesPtc()
        {
            PatientsPat = new HashSet<PatientsPat>();
            UsersUsr = new HashSet<UsersUsr>();
        }


        public int PtcId { get; set; }
        public string PtcName { get; set; }
        public string PtcAddress { get; set; }
        public string PtcCity { get; set; }
        public string PtcZipCode { get; set; }

        public virtual ICollection<PatientsPat> PatientsPat { get; set; }
        public virtual ICollection<UsersUsr> UsersUsr { get; set; }
    }
}
