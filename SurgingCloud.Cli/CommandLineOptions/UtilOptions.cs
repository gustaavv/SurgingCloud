using CommandLine;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("util", aliases: new[] { "u" }, HelpText = "Utility options")]
public class UtilOptions : BaseOptions
{
    [Option("genpwd", Required = false, HelpText = "Generate archive password", SetName = "genpwd")]
    public bool GeneratePassword { get; set; }

    [Option("hash-filename", Required = false, HelpText = "Hash filename", SetName = "hash-filename")]
    public bool HashFilename { get; set; }
    
    [Option("filename", HelpText = "Filename")]
    public string? Filename { get; set; }
}