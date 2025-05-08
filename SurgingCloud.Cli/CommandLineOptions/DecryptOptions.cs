using CommandLine;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("dec", aliases: new[] { "d" }, HelpText = "Decrypt options")]
public class DecryptOptions : BaseOptions
{
    [Option("bypath", HelpText = "Decrypt by file path", Default = false, SetName = "bypath")]
    public bool ByPath { get; set; }

    [Option("encpath", HelpText = "The encrypted file path")]
    public string encFilePath { get; set; }
}