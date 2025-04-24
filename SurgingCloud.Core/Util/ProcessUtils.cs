using System.Diagnostics;

namespace SurgingCloud.Core.Util;

public static class ProcessUtils
{
    /// <summary>
    /// factory method for building a process
    /// </summary>
    public static Process CreateProcess(string filepath, string args)
    {
        var processStartInfo = new ProcessStartInfo()
        {
            FileName = filepath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var process = new Process();
        process.StartInfo = processStartInfo;
        return process;
    }

    /// <summary>
    /// Run a process asynchronously
    /// </summary>
    /// <param name="process"> the process to run</param>
    public static async Task<(int exitCode, string output, string error, Exception? exception)>
        RunProcess(Process process)
    {
        string output = null!, error = null!;
        try
        {
            process.Start();

            // Read the output asynchronously
            var results = await Task.WhenAll(
                process.StandardOutput.ReadToEndAsync(),
                process.StandardError.ReadToEndAsync()
            );
            output = results[0];
            error = results[1];

            await process.WaitForExitAsync();

            return (process.ExitCode, output, error, null);
        }
        catch (Exception ex)
        {
            return (1, output, error, ex);
        }
    }
}