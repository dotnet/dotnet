// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.SignTool
{
    internal readonly struct SignToolArgs
    {
        internal string TempDir { get; }
        internal string MicroBuildCorePath { get; }
        internal bool TestSign { get; }
        internal string DotNetPath { get; }
        internal string MSBuildVerbosity { get; }
        internal string SNBinaryPath { get; }
        internal string LogDir { get; }
        internal string EnclosingDir { get; }
        internal string WixToolsPath { get; }
        internal string TarToolPath { get; }
        internal string PkgToolPath { get; }
        internal int DotNetTimeout { get; }

        internal SignToolArgs(string tempPath, string microBuildCorePath, bool testSign, string dotnetPath, string msbuildVerbosity, string logDir, string enclosingDir, string snBinaryPath, string wixToolsPath, string tarToolPath, string pkgToolPath, int dotnetTimeout)
        {
            TempDir = tempPath;
            MicroBuildCorePath = microBuildCorePath;
            TestSign = testSign;
            DotNetPath = dotnetPath;
            MSBuildVerbosity = msbuildVerbosity;
            LogDir = logDir;
            EnclosingDir = enclosingDir;
            SNBinaryPath = snBinaryPath;
            WixToolsPath = wixToolsPath;
            TarToolPath = tarToolPath;
            PkgToolPath = pkgToolPath;
            DotNetTimeout = dotnetTimeout;
        }
    }
}
