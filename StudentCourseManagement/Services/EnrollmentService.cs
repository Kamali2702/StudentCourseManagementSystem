using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository _repository;
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public EnrollmentService(
        EnrollmentRepository repository,
        StudentRepository studentRepository,
        CourseRepository courseRepository)
    {
        _repository = repository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<int> EnrollAsync(
        int studentId,
        int courseId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "StudentId must be greater than 0.");

        if (courseId <= 0)
            throw new ArgumentException(
                "CourseId must be greater than 0.");

        Student? student =
            await _studentRepository.GetByIdAsync(studentId);

        if (student == null)
            throw new ArgumentException(
                "Student not found.");

        if (!student.IsActive)
            throw new InvalidOperationException(
                "Cannot enroll an inactive student.");

        Course? course =
            await _courseRepository.GetByIdAsync(courseId);

        if (course == null)
            throw new ArgumentException(
                "Course not found.");

        if (!course.IsActive)
            throw new InvalidOperationException(
                "Cannot enroll in an inactive course.");

        return await _repository.AddWithAuditAsync(
            studentId,
            courseId);
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(
        int studentId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "StudentId must be greater than 0.");

        return await _repository.GetByStudentIdAsync(studentId);
    }

    public async Task<List<Enrollment>> GetByCourseIdAsync(
        int courseId)
    {
        if (courseId <= 0)
            throw new ArgumentException(
                "CourseId must be greater than 0.");

        return await _repository.GetByCourseIdAsync(courseId);
    }

    public async Task CompleteAsync(int enrollmentId)
    {
        if (enrollmentId <= 0)
            throw new ArgumentException(
                "EnrollmentId must be greater than 0.");

        await _repository.UpdateStatusAsync(
            enrollmentId,
            "Completed");
    }

    public async Task CancelAsync(int enrollmentId)
    {
        if (enrollmentId <= 0)
            throw new ArgumentException(
                "EnrollmentId must be greater than 0.");

        await _repository.UpdateStatusAsync(
            enrollmentId,
            "Cancelled");
    }
}