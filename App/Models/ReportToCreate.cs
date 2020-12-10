

namespace App.Models
{

    public class ReportToCreate
    {

        public int PatId { get; set; }
        public int CdnId { get; set; }
        public int TmtId { get; set; }
        public int RptRating { get; set; }
        public string RptComment { get; set; }
    }
}
