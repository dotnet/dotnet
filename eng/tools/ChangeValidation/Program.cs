using System;
using System.Diagnostics;
using System.Linq;

namespace ValidateVmrChanges
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var gitDiff = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = "diff --cached --name-only",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                gitDiff.Start();
                string output = gitDiff.StandardOutput.ReadToEnd();
                gitDiff.WaitForExit();

                var changedFiles = output.Split('\n', StringSplitOptions.RemoveEmptyEntries)
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
    }
}
