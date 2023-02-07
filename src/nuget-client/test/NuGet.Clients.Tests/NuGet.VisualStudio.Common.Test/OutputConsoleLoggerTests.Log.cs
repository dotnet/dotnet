// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.Common;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public class Log : LogAndReportErrorTests
        {
            public Log(GlobalServiceProvider sp)
                : base(sp)
            { }

            [Fact]
            public async Task Gets_MSBuild_verbosity_from_shell()
            {
                _outputConsoleLogger.Log(new LogMessage(LogLevel.Debug, "message"));
                await WaitForInitializationAsync();
                _visualStudioShell.Verify(vss => vss.GetPropertyValueAsync("Environment", "ProjectsAndSolution", "MSBuildOutputVerbosity"));
            }

            [Fact]
            public void Non_integer_verbosity_is_interpreted_as_2()
            {
                _outputConsole.Reset();
                _msBuildOutputVerbosity = "string";

                _outputConsoleLogger.Log(new LogMessage(LogLevel.Debug, "message"));

                _outputConsole.VerifyNoOtherCalls();
            }

            [Theory]
            [MemberData(nameof(GetMessageVariationsWhichAreLogged))]
            public async Task Writes_line_to_output_console(LogLevel logLevel, int verbosityLevel)
            {
                _outputConsole.Reset();
                _msBuildOutputVerbosity = verbosityLevel;

                _outputConsoleLogger.Log(new LogMessage(logLevel, "message"));
                await WaitForInitializationAsync();
                _outputConsole.Verify(oc => oc.WriteLineAsync("message"));
            }

            public static IEnumerable<object[]> GetMessageVariationsWhichAreLogged()
            {
                return GetMessageVariations()
                      .Where(v => v.logged)
                      .Select(v => v.variation);
            }

            [Theory]
            [MemberData(nameof(GetMessageVariantsWhichAreNotLogged))]
            public void Does_not_write_line_to_output_console(LogLevel logLevel, int verbosityLevel)
            {
                _outputConsole.Reset();
                _msBuildOutputVerbosity = verbosityLevel;

                _outputConsoleLogger.Log(new LogMessage(logLevel, "message"));

                _outputConsole.VerifyNoOtherCalls();
            }

            public static IEnumerable<object[]> GetMessageVariantsWhichAreNotLogged()
            {
                return GetMessageVariations()
                      .Where(v => !v.logged)
                      .Select(v => v.variation);
            }

            [Theory]
            [MemberData(nameof(GetMessageVariationsWhichAreReported))]
            public async Task Adds_entry_to_error_list(LogLevel logLevel, int verbosityLevel)
            {
                await VerifyThatEntryToErrorListIsAddedAsync(_outputConsoleLogger.Log, logLevel, verbosityLevel);
            }

            [Theory]
            [MemberData(nameof(GetMessageVariationsWhichAreNotReported))]
            public void Does_not_add_entry_to_error_list(LogLevel logLevel, int verbosityLevel)
            {
                VerifyThatEntryToErrorListIsNotAdded(_outputConsoleLogger.Log, logLevel, verbosityLevel);
            }
        }
    }
}
