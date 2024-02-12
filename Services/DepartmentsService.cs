using JobOpeningBackend.Data;
using JobOpeningBackend.DTOs;
using JobOpeningBackend.Exceptions;
using JobOpeningBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningBackend.Services;

public class DepartmentsService : IDepartmentsService
{
    private readonly AppDbContext _dbContext;

    public DepartmentsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await _dbContext.Departments.ToListAsync();
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _dbContext.Departments.FindAsync(id);
    }

    public async Task CreateDepartmentAsync(Department department)
    {
        if (_dbContext.Departments.Any(j => j.Title == department.Title))
        {
            throw new DuplicateTitleException("A department with the same title already exists.");
        }

        _dbContext.Departments.Add(department);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateDepartmentAsync(int id, Department department)
    {
        var existingDepartment =
            await _dbContext.Departments.FindAsync(id)
            ?? throw new NotFoundException("Department not found");

        existingDepartment.Title = department.Title;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var department = await _dbContext.Departments.FindAsync(id);
        if (department == null)
        {
            return;
        }

        _dbContext.Departments.Remove(department);
        await _dbContext.SaveChangesAsync();
    }
}
