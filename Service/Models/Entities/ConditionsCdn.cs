using System.Collections.Generic;


namespace Service.Models
{

    public partial class ConditionsCdn
    {

        public ConditionsCdn()
        {
            ReportsRpt = new HashSet<ReportsRpt>();
        }


        public int CdnId { get; set; }
        public string CdnName { get; set; }
        public string CdnDescription { get; set; }

        public virtual ICollection<ReportsRpt> ReportsRpt { get; set; }
    }
}
