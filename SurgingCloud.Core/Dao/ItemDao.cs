using Dapper;
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
        ";
        conn.Execute(createTableSql);
    }

    public Item? SelectById(long id)
    {
        using var conn = CreateConnection();
        const string sql = "SELECT * FROM Item WHERE Id = @Id";
        return conn.QueryFirstOrDefault<Item>(sql, new { Id = id });
    }

    public Item? SelectByHashBefore(long subjectId, string hashBefore)
    {
        using var conn = CreateConnection();
        const string sql = "SELECT * FROM Item WHERE SubjectId = @SubjectId AND HashBefore = @HashBefore";
        return conn.QueryFirstOrDefault<Item>(sql, new { Id = subjectId, HashBefore = hashBefore });
    }

    public Item? SelectByNameAfter(long subjectId, string nameAfter)
    {
        using var conn = CreateConnection();
        const string sql = "SELECT * FROM Item WHERE SubjectId = @SubjectId AND NameAfter = @NameAfter";
        return conn.QueryFirstOrDefault<Item>(sql, new { Id = subjectId, NameAfter = nameAfter });
    }

    public int Insert(Item item)
    {
        using var conn = CreateConnection();
        const string sql = @"
            INSERT INTO Item(SubjectId, NameBefore, NameAfter, ItemType, HashBefore, HashAfter, SizeBefore, SizeAfter)
            VALUES (@SubjectId, @NameBefore, @NameAfter, @ItemType, @HashBefore, @HashAfter, @SizeBefore, @SizeAfter)
        ";
        return conn.Execute(sql, item);
    }

    public int Delete(long id)
    {
        using var conn = CreateConnection();
        const string sql = "DELETE FROM Item WHERE Id = @Id;";
        return conn.Execute(sql, new { Id = id });
    }
}