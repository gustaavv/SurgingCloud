using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Service;

namespace SurgingCloud.Cli.Controller;

public class DecryptController
{
    private readonly DecryptService _decryptService;

    public DecryptController(DecryptService decryptService)
    {
        _decryptService = decryptService;
    }

    public void DecryptFilepath(DecryptOptions opt)
    {
        if (string.IsNullOrWhiteSpace(opt.encFilePath))
        {
            Console.WriteLine("Please enter the encrypted file path");
        }

        var decryptFilepath = _decryptService.DecryptFilepath(opt.encFilePath, opt.SubjectId);

        Console.WriteLine(
            "The file path is decrypted by each path item. If an path item can not be decrypted, it will be <original item name>");
        Console.WriteLine($"Decrypted file path: {decryptFilepath}");
    }
}