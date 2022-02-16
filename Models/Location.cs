namespace POC.Models
{
    public partial class Location
    {
        public Location()
        {
            Branches = new HashSet<Branch>();
        }

        public int LocationId { get; set; }
        public string Name { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
    }
}
