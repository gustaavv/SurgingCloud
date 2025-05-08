using System.Threading.Tasks;
using JetBrains.Annotations;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Enum;
using SurgingCloud.Core.Service;
using Xunit;

namespace SurgingCloud.Core.Tests.Service;

[TestSubject(typeof(EncryptService))]
public class EncryptServiceTest
{
    private readonly EncryptService _encryptService = new EncryptService(null!, null!, null!, null!);

    [Theory]
    [InlineData("MyPassword", HashAlg.Sha256, "1d35fd2651d0a7ca53b9ea0453484ec325e1ee5b4c3a8993b727c77c2f96cb00")]
    [InlineData("MyPassword", HashAlg.Sha1, "de21192a401fa5d94cf86624a26dcaa8dee8203a")]
    [InlineData("MyPassword", HashAlg.Md5, "922b2f11195c94ec45dd0654ae9d5687")]
    public async Task CreateArchivePassword(string subjectPwd, HashAlg hashAlg, string expectedPwd)
    {
        var subject = new Subject() { Password = subjectPwd, HashAlg = hashAlg };
        var pwd = await _encryptService.CreateArchivePassword(subject);
        Assert.Equal(expectedPwd, pwd);
    }
}