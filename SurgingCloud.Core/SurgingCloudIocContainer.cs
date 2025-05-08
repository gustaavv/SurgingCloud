using Microsoft.Extensions.DependencyInjection;
using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Service;

namespace SurgingCloud.Core;

public static class SurgingCloudIocContainer
{
    public static ServiceCollection Get(string dbPath)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ConfigDao>(_ => new ConfigDao(dbPath));
        services.AddSingleton<SubjectDao>(_ => new SubjectDao(dbPath));
        services.AddSingleton<ItemDao>(_ => new ItemDao(dbPath));
        
        services.AddSingleton<ConfigService>();
        services.AddSingleton<DecryptService>();
        services.AddSingleton<EncryptService>();
        services.AddSingleton<HashService>();
        services.AddSingleton<ItemService>();
        services.AddSingleton<SubjectService>();

        return services;
    }
}