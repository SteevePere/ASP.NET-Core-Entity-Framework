using System.Collections.Generic;


namespace Service.Models
{

    public partial class TreatmentsTmt
    {

        public TreatmentsTmt()
        {
            ReportsRpt = new HashSet<ReportsRpt>();
        }


        public int TmtId { get; set; }
        public string TmtName { get; set; }
        public string TmtDescription { get; set; }

        public virtual ICollection<ReportsRpt> ReportsRpt { get; set; }
    }
}
