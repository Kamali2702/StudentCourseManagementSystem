using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class CourseService
{
    private readonly CourseRepository _repository;

    public CourseService(CourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddAsync(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.CourseName))
            throw new ArgumentException(
                "Course name is required.");

        if (course.DurationMonths <= 0)
            throw new ArgumentException(
                "Duration must be greater than 0.");

        if (course.Fee < 0)
            throw new ArgumentException(
                "Fee cannot be negative.");

        if (course.DepartmentId <= 0)
            throw new ArgumentException(
                "DepartmentId must be greater than 0.");

        return await _repository.AddAsync(course);
    }

    public async Task<List<Course>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Course>> SearchAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Course name is required.");

        return await _repository.SearchAsync(name);
    }

    public async Task<List<Course>> GetByDepartmentAsync(
        int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException(
                "DepartmentId must be greater than 0.");

        return await _repository.GetByDepartmentAsync(departmentId);
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        if (course.CourseId <= 0)
            throw new ArgumentException(
                "CourseId must be greater than 0.");

        if (string.IsNullOrWhiteSpace(course.CourseName))
            throw new ArgumentException(
                "Course name is required.");

        if (course.DurationMonths <= 0)
            throw new ArgumentException(
                "Duration must be greater than 0.");

        if (course.Fee < 0)
            throw new ArgumentException(
                "Fee cannot be negative.");

        if (course.DepartmentId <= 0)
            throw new ArgumentException(
                "DepartmentId must be greater than 0.");

        return await _repository.UpdateAsync(course);
    }

    public async Task<bool> DeactivateAsync(int courseId)
    {
        if (courseId <= 0)
            throw new ArgumentException(
                "CourseId must be greater than 0.");

        return await _repository.DeactivateAsync(courseId);
    }

    public async Task<List<(int CourseId, string CourseName, int EnrollmentCount)>>
        GetEnrollmentCountPerCourseAsync()
    {
        return await _repository.GetEnrollmentCountPerCourseAsync();
    }

    public async Task<List<(int CourseId, string CourseName)>>
        GetCoursesWithoutEnrollmentsAsync()
    {
        return await _repository.GetCoursesWithoutEnrollmentsAsync();
    }
}
