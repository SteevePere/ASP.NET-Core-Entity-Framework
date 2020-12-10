
namespace App.Models
{

    public class UserToCreate
    {

        public int RleId { get; set; }
        public int? PtcId { get; set; }
        public int GdrId { get; set; }
        public string UsrEmail { get; set; }
        public string UsrPassword { get; set; }
        public string UsrFirstName { get; set; }
        public string UsrLastName { get; set; }

        public PracticeToCreate Ptc { get; set; }
    }
}
