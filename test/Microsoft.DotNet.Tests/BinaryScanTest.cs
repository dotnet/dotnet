// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;
using TestUtilities;

namespace Microsoft.DotNet.Tests
{
    [Trait("Category", "BinaryScanTest")]
    public class BinaryScanTest
    {
        private ITestOutputHelper OutputHelper { get; }
        private static readonly string DetectBinariesScriptPath = Path.Combine(Config.RepoRoot, "eng", "detect-binaries.sh");
        public static bool IncludeBinaryScanTest => !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public BinaryScanTest(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        [ConditionalFact(typeof(BinaryScanTest), nameof(IncludeBinaryScanTest))]
        public void ScanForBinaries()
        {
            ExecuteHelper.ExecuteProcessValidateExitCode(
                "/bin/bash",
                $"-c \"{DetectBinariesScriptPath}\"",
                OutputHelper,
                "See https://github.com/dotnet/dotnet/blob/main/src/arcade/Documentation/UnifiedBuild/VMR-Permissible-Sources.md " +
                "for information on how to resolve these failures."
            );
        }
    }
}
