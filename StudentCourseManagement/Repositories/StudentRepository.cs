using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class StudentRepository
{
    public async Task<int> AddAsync(Student student)
    {
        const string sql = """
            INSERT INTO Students
            (
                Name,
                Email,
                Age,
                DepartmentId
            )
            VALUES
            (
                @Name,
                @Email,
                @Age,
                @DepartmentId
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100)
            .Value = student.Name;

        command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 255)
            .Value = student.Email;

        command.Parameters.Add("@Age", System.Data.SqlDbType.Int)
            .Value = student.Age;

        command.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int)
            .Value = student.DepartmentId;

        await connection.OpenAsync();

        object? result =
            await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<Student>> GetAllAsync()
    {
        const string sql = """
            SELECT
                StudentId,
                Name,
                Email,
                Age,
                DepartmentId,
                AdmissionDate,
                IsActive
            FROM Students
            ORDER BY StudentId;
            """;

        List<Student> students = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task<Student?> GetByIdAsync(int studentId)
    {
        const string sql = """
            SELECT
                StudentId,
                Name,
                Email,
                Age,
                DepartmentId,
                AdmissionDate,
                IsActive
            FROM Students
            WHERE StudentId = @StudentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@StudentId", System.Data.SqlDbType.Int)
            .Value = studentId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            };
        }

        return null;
    }

    public async Task<List<Student>> SearchByNameAsync(string name)
    {
        const string sql = """
            SELECT
                StudentId,
                Name,
                Email,
                Age,
                DepartmentId,
                AdmissionDate,
                IsActive
            FROM Students
            WHERE Name LIKE @Name
            ORDER BY Name;
            """;

        List<Student> students = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@Name",
            System.Data.SqlDbType.NVarChar,
            100).Value = $"%{name}%";

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task<List<Student>> GetByDepartmentAsync(
        int departmentId)
    {
        const string sql = """
            SELECT
                StudentId,
                Name,
                Email,
                Age,
                DepartmentId,
                AdmissionDate,
                IsActive
            FROM Students
            WHERE DepartmentId = @DepartmentId
            ORDER BY Name;
            """;

        List<Student> students = new();

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
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        const string sql = """
            UPDATE Students
            SET
                Name = @Name,
                Email = @Email,
                Age = @Age,
                DepartmentId = @DepartmentId
            WHERE StudentId = @StudentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@Name",
            System.Data.SqlDbType.NVarChar,
            100).Value = student.Name;

        command.Parameters.Add(
            "@Email",
            System.Data.SqlDbType.NVarChar,
            255).Value = student.Email;

        command.Parameters.Add(
            "@Age",
            System.Data.SqlDbType.Int).Value = student.Age;

        command.Parameters.Add(
            "@DepartmentId",
            System.Data.SqlDbType.Int).Value = student.DepartmentId;

        command.Parameters.Add(
            "@StudentId",
            System.Data.SqlDbType.Int).Value = student.StudentId;

        await connection.OpenAsync();

        int rows =
            await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<bool> DeactivateAsync(int studentId)
    {
        const string sql = """
            UPDATE Students
            SET IsActive = 0
            WHERE StudentId = @StudentId
              AND IsActive = 1;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@StudentId",
            System.Data.SqlDbType.Int).Value = studentId;

        await connection.OpenAsync();

        int rows =
            await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<List<(int StudentId, string Name, string DepartmentName)>>
        GetStudentsWithDepartmentsAsync()
    {
        const string sql = """
            SELECT
                s.StudentId,
                s.Name,
                s.Email,
                d.DepartmentName
            FROM Students AS s
            INNER JOIN Departments AS d
                ON d.DepartmentId = s.DepartmentId
            ORDER BY s.Name;
            """;

        List<(int StudentId, string Name, string DepartmentName)> result = new();

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
                    reader.GetString(3)
                ));
        }

        return result;
    }

    public async Task<List<(int StudentId, string Name)>>
        GetStudentsWithoutEnrollmentsAsync()
    {
        const string sql = """
            SELECT
                s.StudentId,
                s.Name
            FROM Students AS s
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM Enrollments AS e
                WHERE e.StudentId = s.StudentId
            );
            """;

        List<(int StudentId, string Name)> result = new();

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