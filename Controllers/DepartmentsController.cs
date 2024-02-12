using JobOpeningBackend.Exceptions;
using JobOpeningBackend.Models;
using JobOpeningBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningBackend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Department")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentsService _departmentsService;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(
        IDepartmentsService departmentsService,
        ILogger<DepartmentsController> logger
    )
    {
        _departmentsService = departmentsService;
        _logger = logger;
    }

    // GET: api/v1/departments
    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _departmentsService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

    // GET: api/v1/departments/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await _departmentsService.GetDepartmentByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return Ok(department);
    }

    // POST: api/v1/departments
    [HttpPost]
    public async Task<IActionResult> CreateDepartment(Department department)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _departmentsService.CreateDepartmentAsync(department);
            return CreatedAtAction(
                nameof(GetDepartmentById),
                new { id = department.Id },
                department
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the department.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // PUT: api/v1/departments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, Department department)
    {
        try
        {
            await _departmentsService.UpdateDepartmentAsync(id, department);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // DELETE: api/v1/departments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _departmentsService.DeleteDepartmentAsync(id);
        return NoContent();
    }
}
