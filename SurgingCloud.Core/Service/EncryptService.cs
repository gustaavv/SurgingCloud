using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Core.Service;

public class EncryptService
{
    private static readonly string ARCHIVE_PASSWORD_PREFIX = "SurgingCloud";
    private static readonly int ARCHIVE_PASSWORD_LENGTH = 64;

    public async Task<string> CreateArchivePassword(Subject subject)
    {
        var pwd = ARCHIVE_PASSWORD_PREFIX + subject.Password;
        pwd = await HashUtils.ComputeStringHash(pwd, subject.HashAlg);
        return pwd.Substring(0, Math.Min(pwd.Length, ARCHIVE_PASSWORD_LENGTH));
    }
}