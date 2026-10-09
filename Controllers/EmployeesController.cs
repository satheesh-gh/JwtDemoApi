using JwtDemoApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtDemoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public EmployeesController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .ToListAsync();

            return Ok(employees);
        }

        // Admin only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateEmployee()
        {
            return Ok("Only Admin can create employees.");
        }

        // Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeleteEmployee(int id)
        {
            return Ok($"Admin or Manager deleted employee {id}");
        }
    }
}
