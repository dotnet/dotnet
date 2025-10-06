// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class ExtensionsTests
    {
        [Fact]
        public void TestExtensionsFsromProgramDirLoaded()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var randomTestFolder = TestDirectory.Create())
            {
                var result = CommandRunner.Run(
                    nugetexe,
                    randomTestFolder,
                    "hello");
                result.Output.Should().Be("Hello!" + Environment.NewLine);
            }
        }
    }
}
