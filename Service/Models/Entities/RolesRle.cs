using System.Collections.Generic;


namespace Service.Models
{

    public partial class RolesRle
    {

        public RolesRle()
        {
            UsersUsr = new HashSet<UsersUsr>();
        }


        public int RleId { get; set; }
        public string RleName { get; set; }

        public virtual ICollection<UsersUsr> UsersUsr { get; set; }
    }
}
