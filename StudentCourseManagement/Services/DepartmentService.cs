using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _repository;

    public DepartmentService(DepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
            throw new ArgumentException("Department name is required.");

        if (department.DepartmentName.Length > 100)
            throw new ArgumentException("Department name cannot exceed 100 characters.");

        if (!string.IsNullOrWhiteSpace(department.Location) &&
            department.Location.Length > 100)
            throw new ArgumentException("Location cannot exceed 100 characters.");

        return await _repository.AddAsync(department);
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Department>> SearchAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            throw new ArgumentException("Search text is required.");

        return await _repository.SearchAsync(searchText);
    }

    public async Task<bool> UpdateAsync(Department department)
    {
        if (department.DepartmentId <= 0)
            throw new ArgumentException("DepartmentId must be greater than 0.");

        if (string.IsNullOrWhiteSpace(department.DepartmentName))
            throw new ArgumentException("Department name is required.");

        if (department.DepartmentName.Length > 100)
            throw new ArgumentException("Department name cannot exceed 100 characters.");

        if (!string.IsNullOrWhiteSpace(department.Location) &&
            department.Location.Length > 100)
            throw new ArgumentException("Location cannot exceed 100 characters.");

        return await _repository.UpdateAsync(department);
    }

    public async Task<bool> DeactivateAsync(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("DepartmentId must be greater than 0.");

        return await _repository.DeactivateAsync(departmentId);
    }
    public async Task<List<(int DepartmentId, string DepartmentName, int StudentCount)>>
    GetStudentCountByDepartmentAsync()
    {
        return await _repository.GetStudentCountByDepartmentAsync();
    }
}