using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Vo;
using SurgingCloud.Core.Service;

namespace SurgingCloud.Cli.Controller;

public class UtilController
{
    private readonly EncryptService _encryptService;

    private readonly HashService _hashService;

    public UtilController(EncryptService encryptService, HashService hashService)
    {
        _encryptService = encryptService;
        _hashService = hashService;
    }

    public async Task GeneratePassword(UtilOptions opt)
    {
        if (string.IsNullOrEmpty(opt.Password))
        {
            opt.Cw(OperationResult<object>.Fail("Please enter subject password"));
            return;
        }

        var archivePassword = await _encryptService.CreateArchivePassword(new Subject
        {
            Password = opt.Password,
            HashAlg = opt.HashAlg
        });

        opt.Cw(OperationResult<object>.Ok(
            @$"Subject password: {opt.Password}
Hash algorithm: {opt.HashAlg}
Generated archive password: {archivePassword}"
        ));
    }

    public async Task HashFilename(UtilOptions opt)
    {
        if (string.IsNullOrWhiteSpace(opt.Filename))
        {
            opt.Cw(OperationResult<object>.Fail("Please enter filename"));
            return;
        }

        var hash = await _hashService.HashFilename(opt.Filename, opt.HashAlg);

        opt.Cw(OperationResult<object>.Ok(
            $@"Filename: {opt.Filename}
Hash algorithm: {opt.HashAlg}
Hashed filename: {hash}",
            hash));
    }
}