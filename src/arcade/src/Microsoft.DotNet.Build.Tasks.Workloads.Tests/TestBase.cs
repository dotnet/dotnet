// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public abstract class TestBase
    {
        public static readonly string BaseIntermediateOutputPath = Path.Combine(AppContext.BaseDirectory, "obj", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
        public static readonly string BaseOutputPath = Path.Combine(AppContext.BaseDirectory, "bin", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));

        public static readonly string MsiOutputPath = Path.Combine(BaseOutputPath, "msi");
        public static readonly string TestAssetsPath = Path.Combine(AppContext.BaseDirectory, "testassets");

        public static readonly string WixToolsetPath = Path.Combine(TestAssetsPath, "wix");

        public static readonly string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");
    }
}
