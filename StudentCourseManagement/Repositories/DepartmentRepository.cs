using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class DepartmentRepository
{
    public async Task<int> AddAsync(Department department)
    {
        const string sql = """
            INSERT INTO Departments
            (
                DepartmentName,
                Location,
                IsActive
            )
            VALUES
            (
                @DepartmentName,
                @Location,
                1
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@DepartmentName", System.Data.SqlDbType.NVarChar, 100)
            .Value = department.DepartmentName;

        command.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar, 100)
            .Value = string.IsNullOrWhiteSpace(department.Location)
                ? DBNull.Value
                : department.Location;

        await connection.OpenAsync();

        object? result =
            await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<Department>> GetAllAsync()
    {
        const string sql = """
            SELECT
                DepartmentId,
                DepartmentName,
                Location,
                IsActive
            FROM Departments
            ORDER BY DepartmentId;
            """;

        List<Department> departments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2)
                    ? ""
                    : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return departments;
    }

    public async Task<List<Department>> SearchAsync(
        string searchText)
    {
        const string sql = """
            SELECT
                DepartmentId,
                DepartmentName,
                Location,
                IsActive
            FROM Departments
            WHERE DepartmentName LIKE @SearchText
               OR Location LIKE @SearchText
            ORDER BY DepartmentName;
            """;

        List<Department> departments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@SearchText", System.Data.SqlDbType.NVarChar, 100)
            .Value = $"%{searchText}%";

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2)
                    ? ""
                    : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return departments;
    }

    public async Task<bool> UpdateAsync(
        Department department)
    {
        const string sql = """
            UPDATE Departments
            SET
                DepartmentName = @DepartmentName,
                Location = @Location
            WHERE DepartmentId = @DepartmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int)
            .Value = department.DepartmentId;

        command.Parameters.Add("@DepartmentName", System.Data.SqlDbType.NVarChar, 100)
            .Value = department.DepartmentName;

        command.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar, 100)
            .Value = string.IsNullOrWhiteSpace(department.Location)
                ? DBNull.Value
                : department.Location;

        await connection.OpenAsync();

        int rows =
            await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<bool> DeactivateAsync(
        int departmentId)
    {
        const string sql = """
            UPDATE Departments
            SET IsActive = 0
            WHERE DepartmentId = @DepartmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int)
            .Value = departmentId;

        await connection.OpenAsync();

        int rows =
            await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<List<(int DepartmentId, string DepartmentName, int StudentCount)>>
        GetStudentCountByDepartmentAsync()
    {
        const string sql = """
            SELECT
                d.DepartmentId,
                d.DepartmentName,
                COUNT(s.StudentId) AS StudentCount
            FROM Departments AS d
            LEFT JOIN Students AS s
                ON s.DepartmentId = d.DepartmentId
            GROUP BY
                d.DepartmentId,
                d.DepartmentName
            ORDER BY
                d.DepartmentName;
            """;

        List<(int DepartmentId, string DepartmentName, int StudentCount)> result = new();

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
}