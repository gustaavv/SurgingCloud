using System.Threading.Tasks;
using JetBrains.Annotations;
using SurgingCloud.Core.Model.Enum;
using SurgingCloud.Core.Service;
using Xunit;

namespace SurgingCloud.Core.Tests.Service;

[TestSubject(typeof(HashService))]
public class HashServiceTest
{
    [Theory]
    [InlineData("abc.txt", HashAlg.Sha256, "6c701574374b38dff164")]
    [InlineData("abc.txt", HashAlg.Sha1, "060e4b697d177b0542d4")]
    [InlineData("abc.txt", HashAlg.Md5, "56b6f09c50f3463a6cc3")]
    public async Task HashFilename(string filename, HashAlg hashAlg, string expectedHash)
    {
        var hashService = new HashService();
        var hash = await hashService.HashFilename(filename, hashAlg);
        Assert.Equal(expectedHash, hash);
    }
}