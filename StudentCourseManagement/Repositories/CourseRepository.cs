using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class CourseRepository
{
    public async Task<int> AddAsync(Course course)
    {
        const string sql = """
            INSERT INTO Courses
            (
                CourseName,
                DurationMonths,
                Fee,
                DepartmentId
            )
            VALUES
            (
                @CourseName,
                @DurationMonths,
                @Fee,
                @DepartmentId
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@CourseName",
            System.Data.SqlDbType.NVarChar,
            100).Value = course.CourseName;

        command.Parameters.Add(
            "@DurationMonths",
            System.Data.SqlDbType.Int).Value = course.DurationMonths;

        command.Parameters.Add(
            "@Fee",
            System.Data.SqlDbType.Decimal).Value = course.Fee;

        command.Parameters.Add(
            "@DepartmentId",
            System.Data.SqlDbType.Int).Value = course.DepartmentId;

        await connection.OpenAsync();

        object? result =
            await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<Course>> GetAllAsync()
    {
        const string sql = """
            SELECT
                CourseId,
                CourseName,
                DurationMonths,
                Fee,
                DepartmentId,
                IsActive
            FROM Courses
            ORDER BY CourseId;
            """;

        List<Course> courses = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<Course?> GetByIdAsync(int courseId)
    {
        const string sql = """
            SELECT
                CourseId,
                CourseName,
                DurationMonths,
                Fee,
                DepartmentId,
                IsActive
            FROM Courses
            WHERE CourseId = @CourseId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@CourseId",
            System.Data.SqlDbType.Int).Value = courseId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            };
        }

        return null;
    }

    public async Task<List<Course>> SearchAsync(string name)
    {
        const string sql = """
            SELECT
                CourseId,
                CourseName,
                DurationMonths,
                Fee,
                DepartmentId,
                IsActive
            FROM Courses
            WHERE CourseName LIKE @CourseName
            ORDER BY CourseName;
            """;

        List<Course> courses = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@CourseName",
            System.Data.SqlDbType.NVarChar,
            100).Value = $"%{name}%";

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<List<Course>> GetByDepartmentAsync(
        int departmentId)
    {
        const string sql = """
            SELECT
                CourseId,
                CourseName,
                DurationMonths,
                Fee,
                DepartmentId,
                IsActive
            FROM Courses
            WHERE DepartmentId = @DepartmentId
            ORDER BY CourseName;
            """;

        List<Course> courses = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@DepartmentId",
            System.Data.SqlDbType.Int).Value = departmentId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        const string sql = """
            UPDATE Courses
            SET
                CourseName = @CourseName,
                DurationMonths = @DurationMonths,
                Fee = @Fee,
                DepartmentId = @DepartmentId
            WHERE CourseId = @CourseId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@CourseName",
            System.Data.SqlDbType.NVarChar,
            100).Value = course.CourseName;

        command.Parameters.Add(
            "@DurationMonths",
            System.Data.SqlDbType.Int).Value = course.DurationMonths;

        command.Parameters.Add(
            "@Fee",
            System.Data.SqlDbType.Decimal).Value = course.Fee;

        command.Parameters.Add(
            "@DepartmentId",
            System.Data.SqlDbType.Int).Value = course.DepartmentId;

        command.Parameters.Add(
            "@CourseId",
            System.Data.SqlDbType.Int).Value = course.CourseId;

        await connection.OpenAsync();

        int rows =
            await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<bool> DeactivateAsync(int courseId)
    {
        const string sql = """
            UPDATE Courses
            SET IsActive = 0
            WHERE CourseId = @CourseId
              AND IsActive = 1;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@CourseId",
            System.Data.SqlDbType.Int).Value = courseId;

        await connection.OpenAsync();

        int rows =
            await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<List<(int CourseId, string CourseName, int EnrollmentCount)>>
        GetEnrollmentCountPerCourseAsync()
    {
        const string sql = """
            SELECT
                c.CourseId,
                c.CourseName,
                COUNT(e.EnrollmentId) AS EnrollmentCount
            FROM Courses AS c
            LEFT JOIN Enrollments AS e
                ON e.CourseId = c.CourseId
            GROUP BY
                c.CourseId,
                c.CourseName
            ORDER BY
                EnrollmentCount DESC;
            """;

        List<(int CourseId, string CourseName, int EnrollmentCount)> result = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(
                (
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2)
                ));
        }

        return result;
    }

    public async Task<List<(int CourseId, string CourseName)>>
        GetCoursesWithoutEnrollmentsAsync()
    {
        const string sql = """
            SELECT
                c.CourseId,
                c.CourseName
            FROM Courses AS c
            LEFT JOIN Enrollments AS e
                ON e.CourseId = c.CourseId
            WHERE e.EnrollmentId IS NULL;
            """;

        List<(int CourseId, string CourseName)> result = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(
                (
                    reader.GetInt32(0),
                    reader.GetString(1)
                ));
        }

        return result;
    }
}