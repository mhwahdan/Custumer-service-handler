namespace POC.Models
{
    public partial class Branch
    {
        public Branch()
        {
            Employees = new HashSet<Employee>();
        }

        public int Branchid { get; set; }
        public string Name { get; set; } = null!;
        public int Numberofservices { get; set; }
        public int Locationid { get; set; }
        public virtual Location Location { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
