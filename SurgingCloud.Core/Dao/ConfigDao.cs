using Dapper;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Dao;

public class ConfigDao : BaseDao
{
    public ConfigDao(string dbFilePath) : base(dbFilePath)
    {
    }

    protected override void CreateTablesIfNeeded()
    {
        using var conn = CreateConnection();
        const string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Config
            (
                Id                          INTEGER NOT NULL PRIMARY KEY,
                RarPath                     TEXT    NULL,
                CheckUpdateFrequencyInHours INTEGER NOT NULL,
                BackupFrequencyInDays       INTEGER NOT NULL,
                BackupFolderPath            TEXT    NULL
            );

            INSERT OR IGNORE INTO Config (Id, RarPath, CheckUpdateFrequencyInHours, BackupFrequencyInDays, BackupFolderPath)
            VALUES (1, NULL, 24, 7, NULL);
        ";
        conn.Execute(createTableSql);
    }

    public Config Select()
    {
        using var conn = CreateConnection();
        const string sql = "SELECT * FROM Config WHERE Id = 1";
        return conn.QuerySingle<Config>(sql);
    }

    public int Update(Config config)
    {
        using var conn = CreateConnection();
        const string sql = @"
            UPDATE Config
            SET RarPath = @RarPath,
                CheckUpdateFrequencyInHours = @CheckUpdateFrequencyInHours,
                BackupFrequencyInDays = @BackupFrequencyInDays,
                BackupFolderPath = @BackupFolderPath
            WHERE Id = 1
            ";
        return conn.Execute(sql, config);
    }
}