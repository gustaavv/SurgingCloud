using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Service;

namespace SurgingCloud.Cli.Controller;

public class EncryptController
{
    private readonly EncryptService _encryptService;

    public EncryptController(EncryptService encryptService)
    {
        _encryptService = encryptService;
    }

    public async Task EncryptFile(EncryptOptions opt)
    {
        if (opt.SourcePath == null)
        {
            Console.WriteLine("Please enter the path to the file/folder you want to encrypt");
            return;
        }


        if (opt.OutPath == null)
        {
            Console.WriteLine("Please enter the folder path to generate the encrypted file/folder");
            return;
        }

        if (!Directory.Exists(opt.OutPath))
        {
            Directory.CreateDirectory(opt.OutPath);
            Console.WriteLine($"Output folder created: {opt.OutPath}");
        }

        var (_, msg) = await _encryptService.EncryptItem(opt.SourcePath, opt.SubjectId, opt.OutPath);
        Console.WriteLine(msg);
    }
}