using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class AuditLogRepository
{
    public async Task<List<AuditLog>> GetRecentAsync(int count)
    {
        const string sql = """
            SELECT TOP (@Count)
                AuditId,
                EntityName,
                EntityId,
                Action,
                Details,
                CreatedAt
            FROM AuditLogs
            ORDER BY CreatedAt DESC;
            """;

        List<AuditLog> logs = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@Count", System.Data.SqlDbType.Int)
            .Value = count;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            logs.Add(new AuditLog
            {
                AuditId = reader.GetInt64(0),
                EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2),
                Action = reader.GetString(3),
                Details = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            });
        }

        return logs;
    }
}