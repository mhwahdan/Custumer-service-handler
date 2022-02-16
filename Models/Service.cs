namespace POC.Models
{
    public partial class Service
    {
        public int ServiceId { get; set; }
        public string EmployeeId { get; set; } = null!;
        public int ServicetypeId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int Clientid { get; set; }
        public virtual ServiceType Servicetype { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;
    }
}
