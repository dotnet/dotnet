// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.E2ETest
{
    using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
    using System.Diagnostics;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Spawn of vstest.console with a run tests from the current execting folder.
            var executingLocation = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            System.Diagnostics.Debug.Assert(executingLocation != null, "executingLocation != null");

            // Remove Microsoft.VisualStudio.TestPlatform.TestFramework.*.dll if they are present
            if (File.Exists(Path.Combine(executingLocation, "Microsoft.VisualStudio.TestPlatform.TestFramework.dll")))
            {
                File.Delete(Path.Combine(executingLocation, "Microsoft.VisualStudio.TestPlatform.TestFramework.dll"));
            }

            if (File.Exists(Path.Combine(executingLocation, "Microsoft.VisualStudio.TestPlatform.TestFramework.Extensions.dll")))
            {
                File.Delete(Path.Combine(executingLocation, "Microsoft.VisualStudio.TestPlatform.TestFramework.Extensions.dll"));
            }

            // Start vstest.console with sample test assembly
            var runnerLocation = Path.Combine(executingLocation, "vstest.console.exe");
            var testadapterPath = Path.Combine(executingLocation, "Adapter");
            var testAssembly = Path.Combine(executingLocation, "UnitTestProject.dll");

            var arguments = string.Concat(testAssembly.AddDoubleQuote(), " /testadapterpath:", testadapterPath.AddDoubleQuote());

            var process = new Process
                              {
                                  StartInfo =
                                      {
                                          UseShellExecute = false,
                                          CreateNoWindow = false,
                                          FileName = runnerLocation,
                                          Arguments = arguments
                                      },
                                  EnableRaisingEvents = true
                              };
            process.Start();
            process.WaitForExit();
        }
    }
}
