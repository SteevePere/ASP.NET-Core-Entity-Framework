using System;


namespace Service.Models
{

	public class ReportParameters : QueryStringParameters
	{

		#region Reports
		// uint min value is 0
		public uint RptMinRating { get; set; }
		public uint RptMaxRating { get; set; } = 100;
		// internal to prevent Swagger from showing
		internal bool ValidRptRatingRange => RptMaxRating > RptMinRating;

		// DateTime min value is 01/01/0001 00:00:00
		public DateTime RptMinCreationDatetime { get; set; }
		public DateTime RptMaxCreationDatetime { get; set; } = DateTime.Now;
		// internal to prevent Swagger from showing
		internal bool ValidRptCreationDatetimeRange => RptMaxCreationDatetime > RptMinCreationDatetime;
		#endregion

		#region Patients
		public uint? PatGdrId { get; set; }
		public uint? PatPtcId { get; set; }
		public short? PatIsSmoker { get; set; }
		public short? PatIsPregnant { get; set; }
		#endregion

		#region Treatments
		public uint? TmtId { get; set; }
		#endregion

		#region Conditions
		public uint? CdnId { get; set; }
		#endregion
	}
}
