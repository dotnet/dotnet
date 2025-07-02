// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.IO;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal class DGSpecFactory : IDGSpecFactory
    {
        IEnvironmentVariableReader _environmentVariableReader;

        public DGSpecFactory()
        {
            _environmentVariableReader = new EnvironmentVariableWrapper();
        }

        public DependencyGraphSpec GetDependencyGraphSpec(string project)
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                if (!RunMsbuildTarget(project, tempFile))
                {
                    return null;
                }

                DependencyGraphSpec result = DependencyGraphSpec.Load(tempFile);

                return result;
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        private bool RunMsbuildTarget(string project, string tempFile)
        {
            // When being run from the dotnet CLI, use the same dotnet executable, just in case the dotnet on the PATH is different
            // But when NuGet.CommandLine.XPlat is being called directly, call dotnet on the path, so this code is debuggable.
            string dotnetPath = _environmentVariableReader.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";

            // don't redirect stdout or stderr, so errors are output. But use quiet verbosity, so that success has no output.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(dotnetPath)
            {
                Arguments = $"msbuild " +
                $"\"{project}\" " +
                $"-restore:false " +
                $"-target:GenerateRestoreGraphFile " +
                $"-property:RestoreGraphOutputPath=\"{tempFile}\" " +
                $"-property:RestoreRecursive=false " +
                $"-nologo " +
                $"-verbosity:quiet " +
                $"-tl:false " +
                $"-noautoresponse",
                UseShellExecute = false,
            };

            using var process = Process.Start(processStartInfo);
            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }
}
