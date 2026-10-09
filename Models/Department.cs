namespace JwtDemoApi.Models
{
    public class Department
    {
        public short DepartmentID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public DateTime ModifiedDate { get; set; }
    }
}
