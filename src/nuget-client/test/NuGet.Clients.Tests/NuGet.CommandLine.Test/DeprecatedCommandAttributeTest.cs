// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Xunit;

namespace NuGet.CommandLine.Test
{
    /// <summary>
    /// This test class uses two sample command (greet, and hello) implementations, located in SampleCommandExtensions project
    /// </summary>
    public class DeprecatedCommandAttributeTest
    {
        private readonly string _warning_greet_aternative = string.Format(
                NuGetResources.CommandLine_Warning,
                string.Format(
                    NuGetResources.Warning_CommandDeprecated,
                    "NuGet", "greet", "hello"));

        [Fact]
        public void DeprecatedCommandAttribute_GreetCommandWithDeprecationAttribute_WarningMessage()
        {
            var result = CommandRunner.Run(
                        Util.GetNuGetExePath(),
                        Directory.GetCurrentDirectory(),
                        "greet");

            Util.VerifyResultSuccess(result, _warning_greet_aternative);
            Util.VerifyResultSuccess(result, "Greetings");
        }

        [Fact]
        public void DeprecatedCommandAttribute_GreetingCommandHelpFlag_WarningMessage()
        {
            var result = CommandRunner.Run(
                        Util.GetNuGetExePath(),
                        Directory.GetCurrentDirectory(),
                        "greet -h");

            Util.VerifyResultSuccess(result, _warning_greet_aternative);
            Util.VerifyResultSuccess(result, "help");
        }
    }
}
