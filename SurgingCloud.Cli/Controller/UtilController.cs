using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Service;

namespace SurgingCloud.Cli.Controller;

public class UtilController
{
    private readonly EncryptService _encryptService;

    public UtilController(EncryptService encryptService)
    {
        _encryptService = encryptService;
    }

    public async Task GeneratePassword(UtilOptions opt)
    {
        if (string.IsNullOrEmpty(opt.Password))
        {
            Console.WriteLine("Please enter subject password");
            return;
        }

        var archivePassword = await _encryptService.CreateArchivePassword(new Subject
        {
            Password = opt.Password,
            HashAlg = opt.HashAlg
        });

        Console.WriteLine($"Subject password: {opt.Password}");
        Console.WriteLine($"Hash algorithm: {opt.HashAlg}");
        Console.WriteLine($"Generated archive password: {archivePassword}");
    }
}