using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.DotNet.Helix.JobSender.Test;

public class LegacyRunnerCompatProcessTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(5);

    [Fact]
    public async Task CompatibilityModulesPassPythonBehaviorTests()
    {
        var testPath = Path.Combine(AppContext.BaseDirectory, "LegacyRunnerCompatTests.py");
        var attempted = new List<string>();

        // Helix images disagree on the interpreter name, so try both instead of
        // letting Process.Start throw before the assertion can run.
        foreach (var interpreter in OperatingSystem.IsWindows()
            ? new[] { "python", "python3" }
            : new[] { "python3", "python" })
        {
            attempted.Add(interpreter);

            Process process;
            try
            {
                process = Process.Start(new ProcessStartInfo
                {
                    FileName = interpreter,
                    ArgumentList = { testPath },
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                })!;
            }
            catch (Win32Exception)
            {
                continue;
            }

            using (process)
            {
                var standardOutput = process.StandardOutput.ReadToEndAsync();
                var standardError = process.StandardError.ReadToEndAsync();

                using var cancellation = new CancellationTokenSource(Timeout);
                try
                {
                    await process.WaitForExitAsync(cancellation.Token);
                }
                catch (OperationCanceledException)
                {
                    try
                    {
                        process.Kill(entireProcessTree: true);
                    }
                    catch (InvalidOperationException)
                    {
                        // The process exited between the timeout and the kill.
                    }

                    Assert.Fail(
                        $"Python compatibility tests did not complete within {Timeout.TotalMinutes} minutes.");
                }

                Assert.True(
                    process.ExitCode == 0,
                    $"Python compatibility tests failed with exit code {process.ExitCode}.{Environment.NewLine}" +
                    $"stdout:{Environment.NewLine}{await standardOutput}{Environment.NewLine}" +
                    $"stderr:{Environment.NewLine}{await standardError}");
            }

            return;
        }

        Assert.Fail(
            $"No Python interpreter was found. Tried: {string.Join(", ", attempted)}. " +
            "Python is required to run the legacy runner compatibility tests.");
    }
}
