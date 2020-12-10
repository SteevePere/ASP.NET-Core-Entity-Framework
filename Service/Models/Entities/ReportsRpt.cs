using System;


namespace Service.Models
{

    public partial class ReportsRpt
    {

        public int RptId { get; set; }
        public int PatId { get; set; }
        public int CdnId { get; set; }
        public int TmtId { get; set; }
        public int RptRating { get; set; }
        public string RptComment { get; set; }
        public DateTime RptCreationDatetime { get; set; }
        public DateTime RptEditionDatetime { get; set; }

        public virtual ConditionsCdn Cdn { get; set; }
        public virtual PatientsPat Pat { get; set; }
        public virtual TreatmentsTmt Tmt { get; set; }
    }
}
