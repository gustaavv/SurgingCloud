using SurgingCloud.Core.Model.Enum;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Core.Service;

public class HashService
{
    public async Task<string> HashFilename(string filename, HashAlg hashAlg)
    {
        var hashStr = await HashUtils.ComputeStringHash(filename, hashAlg);
        // returns the first 10 and last 10 (total 20) characters
        return string.Concat(hashStr.AsSpan(0, 10), hashStr.AsSpan(hashStr.Length - 10));
    }
}