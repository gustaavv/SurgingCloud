using System.IO.Compression;
using System.Text;

namespace SurgingCloud.Core.Util;

public static class ArchiveUtils
{
    public static string? RarPath { get; set; }

    public static async Task<int> CompressRar(
        List<string> sources,
        string target,
        string pwd = "",
        int compressRate = 3,
        int? recoveryRate = 10,
        bool solidArchive = false,
        string dictSize = "32"
    )
    {
        if (!File.Exists(RarPath))
        {
            throw new FileNotFoundException($"'{RarPath}' is not a valid path for rar");
        }

        if (File.Exists(target) || Directory.Exists(target))
        {
            throw new Exception($"target already existed: {target}");
        }

        if (target.Contains('"'))
        {
            throw new Exception("No \" in target");
        }


        var filesStr = "";
        foreach (var f in sources)
        {
            var file = f.Trim();
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File does not exist: {file}");
            }

            filesStr += $" \"{file}\" ";
        }

        pwd = pwd.Trim();
        if (pwd != "")
        {
            if (pwd.Length > 126)
            {
                throw new Exception("len(pwd) <= 126");
            }

            if (pwd.Contains('"'))
            {
                throw new Exception("No \" in pwd");
            }

            // -hp Encrypt file and file name
            pwd = $"-hp\"{pwd}\"";
        }

        if (!(compressRate is >= 0 and <= 5))
        {
            throw new Exception("compressRate should be between 0 and 5");
        }

        var recoveryRateStr = "";
        if (recoveryRate.HasValue)
        {
            if (!(recoveryRate.Value >= 1))
            {
                throw new Exception("recoveryRate should be greater than 0");
            }

            recoveryRateStr = $"-rr{recoveryRate}%";
        }

        // Solid archive
        var solidArchiveStr = solidArchive ? "-s" : "";

        // Dictionary size
        dictSize = $"-md{dictSize}";

        // -o- Skip existing files. If output_path already exists, the return code of the program is 10
        // -ep1 Preserve the path of the compressed folder
        // -r0 Recursively compress folders, but do not recurse into the directory when specifying a file
        var argument =
            $" a {solidArchiveStr} {dictSize} -ep1 -o- -m{compressRate} {pwd} {recoveryRateStr} -r0 \"{target}\" {filesStr}";


        using var process = ProcessUtils.CreateProcess(RarPath, argument);
        var (exitCode, _, _, _) = await ProcessUtils.RunProcess(process);
        return exitCode;
    }


    public static async Task<int> ExtractRar(string archive, string outputDir, string pwd = "", bool overwrite = false)
    {
        if (!File.Exists(RarPath))
        {
            throw new FileNotFoundException($"'{RarPath}' is not a valid path for rar");
        }

        if (!File.Exists(archive))
        {
            throw new FileNotFoundException($"'{archive}' is not a valid path for an archive");
        }

        outputDir = outputDir.Trim();

        // dir path must be ended with \
        if (!outputDir.EndsWith("\\"))
        {
            outputDir += "\\";
        }

        pwd = pwd.Trim();
        if (pwd != "")
        {
            if (pwd.Length > 126)
            {
                throw new Exception("len(pwd) <= 126");
            }

            if (pwd.Contains('"'))
            {
                throw new Exception("No \" in pwd");
            }

            // -hp Encrypt file and file name
            pwd = $"-hp\"{pwd}\"";
        }

        var overwriteMode = overwrite ? "-o+" : "-o-";

        var argument = $" x {overwriteMode} {pwd} \"{archive}\" \"{outputDir}\"";

        using var process = ProcessUtils.CreateProcess(RarPath, argument);
        var (exitCode, _, _, _) = await ProcessUtils.RunProcess(process);
        return exitCode;
    }

    public static async Task<int> TestRarPassword(string archive, string pwd = "1")
    {
        if (!File.Exists(RarPath))
        {
            throw new FileNotFoundException($"'{RarPath}' is not a valid path for rar");
        }

        if (!File.Exists(archive))
        {
            throw new FileNotFoundException($"'{archive}' is not a valid path for an archive");
        }

        if (string.IsNullOrWhiteSpace(pwd))
        {
            throw new Exception("empty password is not allowed");
        }

        var argument = $" t -p{pwd} \"{archive}\"";
        using var process = ProcessUtils.CreateProcess(RarPath, argument);
        var (exitCode, _, _, _) = await ProcessUtils.RunProcess(process);
        return exitCode;
    }

    public static async Task<List<string>> ListFilesInRar(string archive, string pwd = "1")
    {
        if (!File.Exists(RarPath))
        {
            throw new FileNotFoundException($"'{RarPath}' is not a valid path for rar");
        }

        if (!File.Exists(archive))
        {
            throw new FileNotFoundException($"'{archive}' is not a valid path for an archive");
        }

        if (string.IsNullOrWhiteSpace(pwd))
        {
            throw new Exception("empty password is not allowed");
        }

        var argument = $" lb -p{pwd} \"{archive}\"";
        using var process = ProcessUtils.CreateProcess(RarPath, argument);
        var (exitCode, output, _, _) = await ProcessUtils.RunProcess(process);
        
        return exitCode == 0 ? output.Split("\n").ToList() : new List<string>();
    }

    public static void CompressZipInMem(string target, List<(string fileContent, string path)> sources)
    {
        using var zipStream = new MemoryStream();

        // must use strict `using` structure, inline `using` is wrong
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (var (fileContent, path) in sources)
            {
                var content = Encoding.UTF8.GetBytes(fileContent);
                var entry = archive.CreateEntry(path);
                using var entryStream = entry.Open();
                entryStream.Write(content, 0, content.Length);
            }
        }

        var zipData = zipStream.ToArray();
        File.WriteAllBytes(target, zipData);
    }

    public static List<(string fileContent, string path)> ExtractZipInMem(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"'{path}' is not a valid path for an archive");
        }

        var ans = new List<(string fileContent, string path)>();

        var zipFileBytes = File.ReadAllBytes(path);
        using var zipStream = new MemoryStream(zipFileBytes);

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
        {
            foreach (var entry in archive.Entries)
            {
                using var fileStream = new MemoryStream();
                using (var entryStream = entry.Open())
                {
                    entryStream.CopyTo(fileStream);
                }

                var fileBytes = fileStream.ToArray();
                var fileContent = Encoding.UTF8.GetString(fileBytes);

                ans.Add((fileContent, entry.FullName));
            }
        }

        return ans;
    }
}