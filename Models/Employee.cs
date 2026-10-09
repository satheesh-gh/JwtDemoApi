namespace JwtDemoApi.Models
{
    public class Employee
    {
        public int BusinessEntityID { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
