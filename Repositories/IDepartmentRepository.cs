using JwtDemoApi.Models;

namespace JwtDemoApi.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    }
}
