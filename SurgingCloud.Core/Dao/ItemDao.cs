using System.Data;
using Dapper;
using Dapper.Transaction;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Dao;

public class ItemDao : BaseDao
{
    public ItemDao(string dbFilePath) : base(dbFilePath)
    {
    }

    protected override void CreateTablesIfNeeded()
    {
        using var conn = CreateConnection();
        const string createTableSql = @"
            CREATE TABLE IF NOT EXISTS Item
            (
                Id         INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                SubjectId  INTEGER NOT NULL,
                NameBefore TEXT    NOT NULL,
                NameAfter  TEXT    NOT NULL,
                ItemType   TEXT    NOT NULL,
                HashBefore TEXT    NOT NULL UNIQUE,
                HashAfter  TEXT    NULL,
                SizeBefore INTEGER NULL,
                SizeAfter  INTEGER NULL,
                CreateAt   TEXT    NOT NULL DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX IF NOT EXISTS idx_item_hashbefore ON Item (HashBefore);

            CREATE INDEX IF NOT EXISTS idx_item_nameafter ON Item (NameAfter);
        ";
        conn.Execute(createTableSql);
    }

    public Item? SelectById(long id, IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Item WHERE Id = @Id";
        if (tx != null)
        {
            return tx.QueryFirstOrDefault<Item>(sql, new { Id = id });
        }

        using var conn = CreateConnection();
        return conn.QueryFirstOrDefault<Item>(sql, new { Id = id });
    }

    public List<Item> SelectBySubjectId(long subjectId, IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Item WHERE SubjectId = @SubjectId";
        if (tx != null)
        {
            return tx.Query<Item>(sql, new { SubjectId = subjectId }).ToList();
        }

        using var conn = CreateConnection();
        return conn.Query<Item>(sql, new { SubjectId = subjectId }).ToList();
    }

    public Item? SelectByHashBefore(long subjectId, string hashBefore, IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Item WHERE SubjectId = @SubjectId AND HashBefore = @HashBefore";
        if (tx != null)
        {
            return tx.QueryFirstOrDefault<Item>(sql, new { SubjectId = subjectId, HashBefore = hashBefore });
        }

        using var conn = CreateConnection();
        return conn.QueryFirstOrDefault<Item>(sql, new { SubjectId = subjectId, HashBefore = hashBefore });
    }

    public Item? SelectByNameAfter(long subjectId, string nameAfter, IDbTransaction? tx = null)
    {
        const string sql = "SELECT * FROM Item WHERE SubjectId = @SubjectId AND NameAfter = @NameAfter";
        if (tx != null)
        {
            return tx.QueryFirstOrDefault<Item>(sql, new { SubjectId = subjectId, NameAfter = nameAfter });
        }

        using var conn = CreateConnection();
        return conn.QueryFirstOrDefault<Item>(sql, new { SubjectId = subjectId, NameAfter = nameAfter });
    }

    public int Insert(Item item, IDbTransaction? tx = null)
    {
        const string sql = @"
            INSERT INTO Item(SubjectId, NameBefore, NameAfter, ItemType, HashBefore, HashAfter, SizeBefore, SizeAfter)
            VALUES (@SubjectId, @NameBefore, @NameAfter, @ItemType, @HashBefore, @HashAfter, @SizeBefore, @SizeAfter)
        ";
        if (tx != null)
        {
            return tx.Execute(sql, item);
        }

        using var conn = CreateConnection();
        return conn.Execute(sql, item);
    }

    public int Delete(long id, IDbTransaction? tx = null)
    {
        const string sql = "DELETE FROM Item WHERE Id = @Id";
        if (tx != null)
        {
            return tx.Execute(sql, new { Id = id });
        }

        using var conn = CreateConnection();
        return conn.Execute(sql, new { Id = id });
    }

    public int DeleteBySubjectId(long subjectId, IDbTransaction? tx = null)
    {
        const string sql = "DELETE FROM Item WHERE SubjectId = @SubjectId";
        if (tx != null)
        {
            return tx.Execute(sql, new { SubjectId = subjectId });
        }

        using var conn = CreateConnection();
        return conn.Execute(sql, new { SubjectId = subjectId });
    }
}