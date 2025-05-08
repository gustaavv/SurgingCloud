using CommandLine;

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

    [Option("name", HelpText = "Subject name")]
    public string? Name { get; set; }


}