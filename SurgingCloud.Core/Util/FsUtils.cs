namespace SurgingCloud.Core.Util;

/**
 * FileSystem Utils
 */
public static class FsUtils
{
    public static string GetLastEntry(string path)
    {
        var ans = Path.GetFileName(path);

        // if the path to a dir ends with / or \
        if (string.IsNullOrEmpty(ans))
        {
            ans = new DirectoryInfo(path).Name;
        }

        return ans;
    }
}