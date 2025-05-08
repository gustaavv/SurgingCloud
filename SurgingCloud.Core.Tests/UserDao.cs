using Dapper;
using SurgingCloud.Core.Dao;

namespace SurgingCloud.Core.Tests;

public class UserDao : BaseDao
{
    public UserDao(string dbFilePath) : base(dbFilePath)
    {
    }

    protected override void CreateTablesIfNeeded()
    {
        using var conn = CreateConnection();
        const string createTableSql = @"
            CREATE TABLE IF NOT EXISTS User
            (
                Id                          INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                userName                    TEXT NOT NULL
            );
        ";
        conn.Execute(createTableSql);
    }
}