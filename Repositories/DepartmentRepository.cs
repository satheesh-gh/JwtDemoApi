using Dapper;
using JwtDemoApi.Models;
using Microsoft.Data.SqlClient;

namespace JwtDemoApi.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IConfiguration _configuration;

        public DepartmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "AdventureWorksConnection");

            using var connection =
                new SqlConnection(connectionString);

            const string sql = """
            SELECT
                DepartmentID,
                Name,
                GroupName,
                ModifiedDate
            FROM HumanResources.Department
            ORDER BY Name
            """;

            return await connection.QueryAsync<Department>(sql);
        }
        public async Task<Department?> GetDepartmentByIdAsync(short id)
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "AdventureWorksConnection");

            using var connection =
                new SqlConnection(connectionString);

            const string sql = """
        SELECT
            DepartmentID,
            Name,
            GroupName,
            ModifiedDate
        FROM HumanResources.Department
        WHERE DepartmentID = @Id
        """;

            return await connection.QueryFirstOrDefaultAsync<Department>(
                sql,
                new { Id = id });
        }
    }
}
