using CommandLine;
using SurgingCloud.Core.Model.Enum;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("subject", aliases: new[] { "sub", "s" }, HelpText = "Subject options")]
public class SubjectOptions : BaseOptions
{
    [Option('n', "new", HelpText = "Create new subject", Default = false, SetName = "new")]
    public bool CreateSubject { get; set; }

    [Option('l', "list", HelpText = "List all subjects", Default = false, SetName = "list")]
    public bool ListSubjects { get; set; }

    [Option('g', "get", HelpText = "Get subject", Default = false, SetName = "get")]
    public bool GetSubject { get; set; }

    [Option('d', "delete", HelpText = "Delete subject", Default = false, SetName = "delete")]
    public bool DeleteSubject { get; set; }

    [Option("list-items", HelpText = "List all items in a subject", Default = false, SetName = "list-items")]
    public bool ListItems { get; set; }

    [Option("name", HelpText = "Subject name")]
    public string? Name { get; set; }

    [Option("enc-method", Default = EncMethod.Rar, HelpText = "Encryption method enum value. 0: RAR")]
    public EncMethod EncMethod { get; set; }
}