using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class EnrollmentRepository
{
    public async Task<int> AddWithAuditAsync(int studentId, int courseId)
    {
        const string enrollmentSql = """
            INSERT INTO Enrollments (StudentId, CourseId)
            VALUES (@StudentId, @CourseId);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        const string auditSql = """
            INSERT INTO AuditLogs
            (
                EntityName,
                EntityId,
                Action,
                Details
            )
            VALUES
            (
                @EntityName,
                @EntityId,
                @Action,
                @Details
            );
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using SqlTransaction transaction =
            (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            int enrollmentId;

            await using (SqlCommand enrollmentCommand =
                new(enrollmentSql, connection, transaction))
            {
                enrollmentCommand.Parameters.Add("@StudentId", System.Data.SqlDbType.Int)
                    .Value = studentId;

                enrollmentCommand.Parameters.Add("@CourseId", System.Data.SqlDbType.Int)
                    .Value = courseId;

                object? result =
                    await enrollmentCommand.ExecuteScalarAsync();

                enrollmentId = Convert.ToInt32(result);
            }

            await using (SqlCommand auditCommand =
                new(auditSql, connection, transaction))
            {
                auditCommand.Parameters.Add("@EntityName", System.Data.SqlDbType.NVarChar, 50)
                    .Value = "Enrollment";

                auditCommand.Parameters.Add("@EntityId", System.Data.SqlDbType.Int)
                    .Value = enrollmentId;

                auditCommand.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 20)
                    .Value = "INSERT";

                auditCommand.Parameters.Add("@Details", System.Data.SqlDbType.NVarChar, 500)
                    .Value = $"Student {studentId} enrolled in course {courseId}.";

                await auditCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return enrollmentId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        const string sql = """
            SELECT
                EnrollmentId,
                StudentId,
                CourseId,
                EnrolledAt,
                Status
            FROM Enrollments
            ORDER BY EnrollmentId;
            """;

        List<Enrollment> enrollments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
    {
        const string sql = """
            SELECT
                EnrollmentId,
                StudentId,
                CourseId,
                EnrolledAt,
                Status
            FROM Enrollments
            WHERE StudentId = @StudentId
            ORDER BY EnrollmentId;
            """;

        List<Enrollment> enrollments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@StudentId", System.Data.SqlDbType.Int)
            .Value = studentId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task<List<Enrollment>> GetByCourseIdAsync(int courseId)
    {
        const string sql = """
            SELECT
                EnrollmentId,
                StudentId,
                CourseId,
                EnrolledAt,
                Status
            FROM Enrollments
            WHERE CourseId = @CourseId
            ORDER BY EnrollmentId;
            """;

        List<Enrollment> enrollments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@CourseId", System.Data.SqlDbType.Int)
            .Value = courseId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task UpdateStatusAsync(int enrollmentId, string status)
    {
        const string sql = """
            UPDATE Enrollments
            SET Status = @Status
            WHERE EnrollmentId = @EnrollmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
            .Value = status;

        command.Parameters.Add("@EnrollmentId", System.Data.SqlDbType.Int)
            .Value = enrollmentId;

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}