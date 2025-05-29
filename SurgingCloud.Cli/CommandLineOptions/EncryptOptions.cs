using CommandLine;

namespace SurgingCloud.Cli.CommandLineOptions;

[Verb("enc", aliases: new[] { "e" }, HelpText = "Encrypt options")]
public class EncryptOptions : BaseOptions
{
    [Option("byfile", HelpText = "Encrypt a file", Default = false, SetName = "byfile")]
    public bool ByFile { get; set; }

    [Option("byfolder", HelpText = "Encrypt a folder", Default = false, SetName = "byfolder")]
    public bool ByFolder { get; set; }

    [Option("src", HelpText = "The path to the file/folder you want to encrypt")]
    public string? SourcePath { get; set; }

    [Option("out", HelpText = "The folder path to generate the encrypted file/folder")]
    public string? OutPath { get; set; }

    [Option("overwrite", HelpText = "Overwrite existing files in the out path with the same name", Default = false)]
    public bool Overwrite { get; set; }

    [Option("ignore-dup", HelpText = "Ignore encryption if there is an item with the same hashBefore in the database",
        Default = false)]
    public bool IgnoreIfDuplicateInDb { get; set; }
}