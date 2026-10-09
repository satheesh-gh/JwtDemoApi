using JwtDemoApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentsController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _repository.GetAllDepartmentsAsync();

            return Ok(departments);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(short id)
        {
            var department =
                await _repository.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    Message = $"Department with ID {id} not found."
                });
            }

            return Ok(department);
        }

    }
}
