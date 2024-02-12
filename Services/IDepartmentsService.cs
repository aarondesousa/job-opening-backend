using JobOpeningBackend.Models;

namespace JobOpeningBackend.Services;

public interface IDepartmentsService
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<Department?> GetDepartmentByIdAsync(int id);
    Task CreateDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(int id, Department department);
    Task DeleteDepartmentAsync(int id);
}
