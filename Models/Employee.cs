using Microsoft.AspNetCore.Identity;


namespace POC.Models
{
    public partial class Employee : IdentityUser
    {
        public int Branchid { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual ICollection<Service> services { get; set; }
    }
}
