using CommandLine;
using SurgingCloud.Core.Model.Enum;
using SurgingCloud.Core.Model.Vo;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Cli.CommandLineOptions;

public class BaseOptions
{
    [Option("db", HelpText = "Path to DB file", Required = true)]
    public string DbFilePath { get; set; }

    [Option("sid", HelpText = "Subject Id", Default = -1)]
    public long SubjectId { get; set; }

    [Option("iid", HelpText = "Item Id", Default = -1)]
    public long ItemId { get; set; }

    [Option("pwd", HelpText = "Subject password")]
    public string? Password { get; set; }

    [Option("hashAlg", Default = 0, HelpText = "Hash algorithm enum value. 0: SHA256; 1: SHA1; 2: MD5")]
    public HashAlg HashAlg { get; set; }

    [Option("out-json", Default = false, HelpText = "Output int JSON format")]
    public bool JsonFormatOutput { get; set; }

    [Option("bkp-db", Default = false, HelpText = "Backup database before operation")]
    public bool BackupDb { get; set; }

    public void Cw<T>(OperationResult<T> result)
    {
        if (JsonFormatOutput)
        {
            Console.WriteLine(JsonUtils.ToStr(result, pretty: true));
        }
        else
        {
            Console.WriteLine(result.Message);
        }
    }
}