using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repository;

    public AuditLogService(AuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AuditLog>> GetRecentAsync(int count)
    {
        if (count <= 0)
            throw new ArgumentException(
                "Count must be greater than 0.");

        return await _repository.GetRecentAsync(count);
    }
}