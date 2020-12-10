using System;


namespace App.Models
{

    public class Users
    {

        public int UsrId { get; set; }
        public int RleId { get; set; }
        public int? PtcId { get; set; }
        public int GdrId { get; set; }
        public string UsrEmail { get; set; }
        public string UsrPassword { get; set; }
        public string UsrFirstName { get; set; }
        public string UsrLastName { get; set; }
        public short UsrActive { get; set; }
        public DateTime UsrCreationDatetime { get; set; }
        public DateTime UsrEditDatetime { get; set; }
    }
}
