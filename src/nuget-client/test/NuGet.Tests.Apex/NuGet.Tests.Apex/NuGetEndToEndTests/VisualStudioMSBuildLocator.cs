// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;

namespace NuGet.Tests.Apex
{
    internal static class VisualStudioMSBuildLocator
    {
        private static readonly Lazy<string?> MSBuildPath = new(FindMSBuildWithVsWhere);

        internal static string? GetMSBuildPath()
        {
            return MSBuildPath.Value;
        }

        private static string? FindMSBuildWithVsWhere()
        {
            string vswherePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Microsoft Visual Studio",
                "Installer",
                "vswhere.exe");

            if (!File.Exists(vswherePath))
            {
                return null;
            }

            CommandRunnerResult result = CommandRunner.Run(
                filename: vswherePath,
                arguments: "-latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\MSBuild.exe");

            if (!result.Success)
            {
                return null;
            }

            using var reader = new StringReader(result.Output);
            string? line = reader.ReadLine();

            return !string.IsNullOrEmpty(line) && File.Exists(line) ? line : null;
        }
    }
}
