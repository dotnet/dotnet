using System.Diagnostics;

namespace Valleysoft.DockerCredsProvider;

internal interface IProcessService
{
    int Run(ProcessStartInfo startInfo, string? input, Action<string?> outputDataReceived, Action<string?> errorDataReceived);
}

internal class ProcessService : IProcessService
{
    public int Run(ProcessStartInfo startInfo, string? input, Action<string?> outputDataReceived, Action<string?> errorDataReceived)
    {
        Process process = new()
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += (sender, args) => outputDataReceived(args.Data);
        process.ErrorDataReceived += (sender, args) => errorDataReceived(args.Data);

        process.Start();

        if (input is not null)
        {
            process.StandardInput.WriteLine(input);
            process.StandardInput.Close();
        }

        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();

        return process.ExitCode;
    }
}
