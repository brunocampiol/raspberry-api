using RaspberryPi.Domain.Core;
using System.Diagnostics;

namespace RaspberryPi.Domain.Services
{
    public class ProcessService
    {
        public Result<string> RunBashCommand(string command, string arguments)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"{command} {arguments}\"";
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            using (var process = Process.Start(processInfo))
            {
                if (process is null)
                {
                    return Result<string>.Failure("Failed to start process");
                }

                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    return Result<string>.Failure($"Error: {error}");
                }

                return Result<string>.Success(output);
            }
        }
    }
}