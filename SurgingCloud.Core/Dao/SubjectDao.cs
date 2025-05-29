using System.Data;
using Dapper;
using Dapper.Transaction;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Dao;

public class SubjectDao : BaseDao
{
    public SubjectDao(string dbFilePath) : base(dbFilePath)
    {
    }

    protected override void CreateTablesIfNeeded()
    {
        using var conn = CreateConnection();
        const string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Subject
            (
                Id       INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Name     TEXT    NOT NULL UNIQUE,
                Password TEXT    NOT NULL,
                HashAlg  TEXT    NOT NULL,
                CreateAt TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdateAt TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP
            );
        ";
        conn.Execute(createTableSql);
    }

    public List<Subject> Select(IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Subject";
        if (tx != null)
        {
            return tx.Query<Subject>(sql).ToList();
        }

        using var conn = CreateConnection();
        return conn.Query<Subject>(sql).ToList();
    }

    public Subject? SelectById(long id, IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Subject WHERE Id = @Id";
        if (tx != null)
        {
            return tx.QueryFirstOrDefault<Subject>(sql, new { Id = id });
        }

        using var conn = CreateConnection();
        return conn.QueryFirstOrDefault<Subject>(sql, new { Id = id });
    }

    public Subject? SelectByName(string name, IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Subject WHERE Name = @Name";
        if (tx != null)
        {
            return tx.QueryFirstOrDefault<Subject>(sql, new { Name = name });
        }

        using var conn = CreateConnection();
        return conn.QueryFirstOrDefault<Subject>(sql, new { Name = name });
    }

    public int Insert(Subject subject, IDbTransaction? tx = null)
    {
        const string sql = @"
            INSERT INTO Subject (Name, Password, HashAlg)
            VALUES (@Name, @Password, @HashAlg)
        ";
        if (tx != null)
        {
            return tx.Execute(sql, subject);
        }

        using var conn = CreateConnection();
        return conn.Execute(sql, subject);
    }

    public int Update(Subject subject, IDbTransaction? tx = null)
    {
        const string sql = @"
            UPDATE Subject
            SET Name = @Name, Password = @Password, HashAlg = @HashAlg, UpdateAt = CURRENT_TIMESTAMP
            WHERE Id = @Id
        ";
        if (tx != null)
        {
            return tx.Execute(sql, subject);
        }

        using var conn = CreateConnection();
        return conn.Execute(sql, subject);
    }

    public int Delete(long id, IDbTransaction? tx = null)
    {
        const string sql = "DELETE FROM Subject WHERE Id = @Id";
        if (tx != null)
        {
            return tx.Execute(sql, new { Id = id });
        }

        using var conn = CreateConnection();
        return conn.Execute(sql, new { Id = id });
    }
}