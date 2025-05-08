using System;
using System.IO;
using Dapper.Transaction;
using Xunit;
using Xunit.Abstractions;

namespace SurgingCloud.Core.Tests;

public class TxTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TxTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void PrintCwd()
    {
        _testOutputHelper.WriteLine(Directory.GetCurrentDirectory());
    }

    [Fact]
    public void txTest()
    {
        var userDao = new UserDao("testTx.db");
        using (var conn = userDao.CreateConnection())
        {
            conn.Open();
            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    tx.Execute("INSERT INTO User(username) VALUES ('alice')");

                    // throw new Exception("test rollback");
                    
                    tx.Execute("INSERT INTO User(username) VALUES ('bob')");
                    
                    
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
    
    
}