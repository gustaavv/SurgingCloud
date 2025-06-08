using CommandLine;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("item", aliases: new[] { "i" }, HelpText = "Item options")]
public class ItemOptions : BaseOptions
{
    [Option('g', "get", HelpText = "Get item", Default = false, SetName = "get")]
    public bool GetItem { get; set; }

    [Option("iid", HelpText = "Item Id")] 
    public long ItemId { get; set; }
}