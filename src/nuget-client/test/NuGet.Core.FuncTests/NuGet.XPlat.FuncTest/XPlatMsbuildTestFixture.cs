// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using NuGet.Test.Utility;
using NuGet.Versioning;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatMsbuildTestFixture : IDisposable
    {
        private readonly string _dotnetCli = TestFileSystemUtility.GetDotnetCli();

        public XPlatMsbuildTestFixture()
        {
            var cliDirectory = Directory.GetParent(_dotnetCli);
            var msBuildSdksPath = Path.Combine(GetLatestSdkPath(cliDirectory.FullName), "Sdks");
            Environment.SetEnvironmentVariable("MSBuildSDKsPath", msBuildSdksPath);
        }

        private static string GetLatestSdkPath(string dotnetRoot)
        {
            return new DirectoryInfo(Path.Combine(dotnetRoot, "sdk"))
                .EnumerateDirectories()
                .Where(d => NuGetVersion.TryParse(d.Name, out _))
                .OrderByDescending(d => NuGetVersion.Parse(d.Name))
                .First().FullName;
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("MSBuildSDKsPath", null);
        }
    }
}
