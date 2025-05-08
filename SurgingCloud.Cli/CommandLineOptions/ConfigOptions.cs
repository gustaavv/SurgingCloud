using CommandLine;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("config", aliases: new[] { "conf", "c" }, HelpText = "Config options")]
public class ConfigOptions : BaseOptions
{
    [Option('u', "update", HelpText = "Update config", Default = false, SetName = "update")]
    public bool UpdateConfig { get; set; }

    [Option('g', "get", HelpText = "Get current config", Default = false, SetName = "get")]
    public bool GetConfig { get; set; }

    [Option("rar", HelpText = "Path to rar.exe")]
    public string RarPath { get; set; }
}