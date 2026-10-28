using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(StudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> RegisterAsync(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required.");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Student email is required.");

        if (student.Age < 17 || student.Age > 60)
            throw new ArgumentException(
                "Student age must be between 17 and 60.");

        if (student.DepartmentId <= 0)
            throw new ArgumentException(
                "DepartmentId must be greater than 0.");

        return await _repository.AddAsync(student);
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Student?> GetByIdAsync(int studentId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "StudentId must be greater than 0.");

        return await _repository.GetByIdAsync(studentId);
    }

    public async Task<List<Student>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Search name is required.");

        return await _repository.SearchByNameAsync(name);
    }

    public async Task<List<Student>> GetByDepartmentAsync(
        int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException(
                "DepartmentId must be greater than 0.");

        return await _repository.GetByDepartmentAsync(departmentId);
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        if (student.StudentId <= 0)
            throw new ArgumentException(
                "StudentId must be greater than 0.");

        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException(
                "Student name is required.");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException(
                "Student email is required.");

        if (student.Age < 17 || student.Age > 60)
            throw new ArgumentException(
                "Student age must be between 17 and 60.");

        if (student.DepartmentId <= 0)
            throw new ArgumentException(
                "DepartmentId must be greater than 0.");

        return await _repository.UpdateAsync(student);
    }

    public async Task<bool> DeactivateAsync(int studentId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "StudentId must be greater than 0.");

        return await _repository.DeactivateAsync(studentId);
    }

    public async Task<List<(int StudentId, string Name, string DepartmentName)>>
        GetStudentsWithDepartmentsAsync()
    {
        return await _repository.GetStudentsWithDepartmentsAsync();
    }

    public async Task<List<(int StudentId, string Name)>>
        GetStudentsWithoutEnrollmentsAsync()
    {
        return await _repository.GetStudentsWithoutEnrollmentsAsync();
    }
}
