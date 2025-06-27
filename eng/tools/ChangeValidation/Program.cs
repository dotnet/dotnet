using System;
using System.Diagnostics;
using System.Linq;

namespace ValidateVmrChanges
{
    class Program
    {
        static int Main(string[] args)
        {
            string targetBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH");

            if (string.IsNullOrEmpty(targetBranch))
            {
                Console.WriteLine("Error: The target branch is not specified. Please set the SYSTEM_PULLREQUEST_TARGETBRANCH environment variable.");
                return 1;
            }

            try
            {
                // Fetch the target branch
                RunGitCommand($"fetch origin {targetBranch}");

                // Diff against the target branch
                string diffOutput = RunGitCommand($"diff --name-only origin/{targetBranch}...HEAD");

                Console.WriteLine("Git diff output:\n");
                Console.Write(diffOutput);

                var changedFiles = diffOutput
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim())
                    .ToList();

                var submoduleChanges = changedFiles
                    .Where(f => f.Equals(".gitmodules", StringComparison.OrdinalIgnoreCase) ||
                                f.Contains("submodules/") ||
                                f.EndsWith("/.gitmodules"))
                    .ToList();

                if (submoduleChanges.Any())
                {
                    Console.WriteLine("Failure: Submodule-related changes detected:");
                    foreach (var file in submoduleChanges)
                    {
                        Console.WriteLine($" - {file}");
                    }
                    Console.WriteLine("Changes to these files are not permitted.");
                    return 1;
                }

                Console.WriteLine("Success: No submodule-related changes detected.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while detecting submodule changes: {ex.Message}");
                return 1;
            }
        }

        private static string RunGitCommand(string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed: {arguments}\n{err}");
            }

            return output;
        }
    }
}
