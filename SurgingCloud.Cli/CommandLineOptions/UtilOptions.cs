using CommandLine;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("util", aliases: new[] { "u" }, HelpText = "Utility options")]
public class UtilOptions : BaseOptions
{
    [Option("genpwd", Required = false, HelpText = "Generate archive password", SetName = "genpwd")]
    public bool GeneratePassword { get; set; }
}