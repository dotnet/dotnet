// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
        public static bool IncludeBinaryScanTest => !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public BinaryScanTest(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        [ConditionalFact(typeof(BinaryScanTest), nameof(IncludeBinaryScanTest))]
        public void ScanForBinaries()
        {
            Assert.True(
                File.Exists(Config.BinariesReportFile),
                "The binaries report file does not exist. Binary detection may have failed with build errors.");

            string detectedBinaries = File.ReadAllText(Config.BinariesReportFile);

            Assert.True(
                string.IsNullOrWhiteSpace(detectedBinaries),
                "The following binaries were detected:\n" + detectedBinaries +
                "\nSee https://github.com/dotnet/dotnet/blob/main/docs/VMR-Permissible-Sources.md " +
                "for information on how to resolve these failures.");
        }
    }
}
