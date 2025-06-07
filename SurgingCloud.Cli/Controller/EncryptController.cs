using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Vo;
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
            opt.Cw(OperationResult<object>.Fail("Please enter the path to the file/folder you want to encrypt"));
            return;
        }

        if (opt.OutPath == null)
        {
            opt.Cw(OperationResult<object>.Fail("Please enter the folder path to generate the encrypted file/folder"));
            return;
        }

        if (!Directory.Exists(opt.OutPath))
        {
            Directory.CreateDirectory(opt.OutPath);
            // Console.WriteLine($"Output folder created: {opt.OutPath}");
        }

        var result =
            await _encryptService.EncryptItem(opt.SourcePath, opt.SubjectId, opt.OutPath, opt.IgnoreIfDuplicateInDb);
        opt.Cw(result);
    }
}