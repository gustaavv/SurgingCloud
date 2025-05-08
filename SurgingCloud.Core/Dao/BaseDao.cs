using System.Data;
using System.Data.SQLite;
// using Serilog;

namespace SurgingCloud.Core.Dao;

public abstract class BaseDao
{
    public string DbFilePath { get; set; }

    private string ConnectionString { get; set; }

    protected BaseDao(string dbFilePath)
    {
        DbFilePath = dbFilePath;
        // if (!File.Exists(dbFilePath))
        // {
        //     Log.Information("Creating db file, filepath = {dbFilePath}", dbFilePath);
        // }

        ConnectionString = $"Data Source={dbFilePath};Version=3;";
        CreateTablesIfNeeded();
    }

    protected abstract void CreateTablesIfNeeded();

    public IDbConnection CreateConnection()
    {
        return new SQLiteConnection(ConnectionString);
    }
}