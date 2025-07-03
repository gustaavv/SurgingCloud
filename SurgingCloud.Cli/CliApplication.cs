using System.Reflection;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Cli.Controller;
using SurgingCloud.Core;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Cli;

public static class CliApplication
{
    private static Type[] LoadVerbs()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
    }

    public static async Task Main(string[] args)
    {
        var types = LoadVerbs();
        var parserResult = Parser.Default.ParseArguments(args, types);
        await parserResult.WithParsedAsync(Run);
        await parserResult.WithNotParsedAsync(HandleError);
    }

    private static ServiceProvider BuildIocContainer(string dbPath)
    {
        var serviceCollection = SurgingCloudIocContainer.Get(dbPath);

        serviceCollection.AddSingleton<ConfigController>();
        serviceCollection.AddSingleton<DecryptController>();
        serviceCollection.AddSingleton<EncryptController>();
        serviceCollection.AddSingleton<SubjectController>();
        serviceCollection.AddSingleton<ItemController>();
        // Even though UtilController currently does not need access to database, I still put it here for convenience
        serviceCollection.AddSingleton<UtilController>();

        return serviceCollection.BuildServiceProvider();
    }

    private static void BackupDb(BaseOptions baseOptions)
    {
        var dir = Directory.GetParent(baseOptions.DbFilePath)!.FullName;
        var dbFilename = FsUtils.GetLastEntry(baseOptions.DbFilePath);
        var now = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        var bkpFilename = dbFilename + $".{now}.bkp";
        var bkpFilepath = Path.Combine(dir, bkpFilename);
        File.Copy(baseOptions.DbFilePath, bkpFilepath);
        if (!baseOptions.JsonFormatOutput)
        {
            Console.WriteLine($"Backup database file at {bkpFilepath}");
        }
    }

    private static async Task Run(object obj)
    {
        var baseOptions = (BaseOptions)obj;
        if (string.IsNullOrWhiteSpace(baseOptions.DbFilePath))
        {
            Console.WriteLine("Please specify a valid database file path");
            return;
        }

        var dbExists = File.Exists(baseOptions.DbFilePath);
        if (!baseOptions.JsonFormatOutput)
        {
            if (!dbExists)
            {
                Console.WriteLine($"Create a new database file at {baseOptions.DbFilePath}");
            }
            else
            {
                Console.WriteLine($"Using existing database file at {baseOptions.DbFilePath}");
            }
        }

        if (dbExists && baseOptions.BackupDb)
        {
            BackupDb(baseOptions);
        }

        var iocContainer = BuildIocContainer(baseOptions.DbFilePath);

        // dispatch request to controller
        switch (obj)
        {
            case ConfigOptions opt:
                var configController = iocContainer.GetRequiredService<ConfigController>();
                if (opt.UpdateConfig)
                {
                    configController.UpdateConfig(opt);
                    return;
                }
                else if (opt.GetConfig)
                {
                    configController.GetConfig();
                    return;
                }

                break;
            case DecryptOptions opt:
                var decryptController = iocContainer.GetRequiredService<DecryptController>();
                if (opt.ByPath)
                {
                    decryptController.DecryptFilepath(opt);
                    return;
                }

                break;
            case EncryptOptions opt:
                var encryptController = iocContainer.GetRequiredService<EncryptController>();
                if (opt.ByFile)
                {
                    await encryptController.EncryptFile(opt);
                    return;
                }

                break;
            case SubjectOptions opt:
                var subjectController = iocContainer.GetRequiredService<SubjectController>();
                if (opt.CreateSubject)
                {
                    subjectController.CreateSubject(opt);
                    return;
                }
                else if (opt.ListSubjects)
                {
                    subjectController.ListAllSubjects(opt);
                    return;
                }
                else if (opt.GetSubject)
                {
                    await subjectController.GetSubject(opt);
                    return;
                }
                else if (opt.DeleteSubject)
                {
                    subjectController.DeleteSubject(opt);
                    return;
                }
                else if (opt.ListItems)
                {
                    subjectController.ListAllItems(opt);
                    return;
                }

                break;
            case ItemOptions opt:
                var itemController = iocContainer.GetRequiredService<ItemController>();
                if (opt.GetItem)
                {
                    itemController.GetItem(opt);
                    return;
                }

                break;
            case UtilOptions opt:
                var utilController = iocContainer.GetRequiredService<UtilController>();
                if (opt.GeneratePassword)
                {
                    await utilController.GeneratePassword(opt);
                    return;
                }

                break;
        }

        Console.WriteLine("No operation provided. Check valid operation using --help");
    }

    private static async Task HandleError(IEnumerable<Error> errors)
    {
        foreach (var e in errors)
        {
            Console.WriteLine(e);
        }
    }
}