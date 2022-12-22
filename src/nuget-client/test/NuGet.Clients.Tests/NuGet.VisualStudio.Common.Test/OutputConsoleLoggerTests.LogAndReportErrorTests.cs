// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NuGet.Common;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public abstract class LogAndReportErrorTests : OutputConsoleLoggerTests
        {
            protected void VerifyThatEntryToErrorListIsAdded(Action<LogMessage> action, LogLevel logLevel, int verbosityLevel)
            {
                ErrorListTableEntry[] errorListTableEntries = null;

                _errorList.Reset();
                _errorList.Setup(el => el.AddNuGetEntries(It.IsAny<ErrorListTableEntry[]>()))
                          .Callback<ErrorListTableEntry[]>(elte => errorListTableEntries = elte);

                _msBuildOutputVerbosity = verbosityLevel;

                action(new LogMessage(logLevel, "message"));

                _errorList.Verify(el => el.AddNuGetEntries(It.IsAny<ErrorListTableEntry[]>()));

                errorListTableEntries.Length.Should().Be(1);
                errorListTableEntries[0].Message.Message.Should().Be("message");
                errorListTableEntries[0].Message.Level.Should().Be(logLevel);
            }

            public static IEnumerable<object[]> GetMessageVariationsWhichAreReported()
            {
                return GetMessageVariations()
                      .Where(v => v.reported)
                      .Select(v => v.variation);
            }

            protected void VerifyThatEntryToErrorListIsNotAdded(Action<LogMessage> action, LogLevel logLevel, int verbosityLevel)
            {
                _errorList.Reset();
                _msBuildOutputVerbosity = verbosityLevel;

                action(new LogMessage(logLevel, "message"));

                _errorList.VerifyNoOtherCalls();
            }

            public static IEnumerable<object[]> GetMessageVariationsWhichAreNotReported()
            {
                return GetMessageVariations()
                      .Where(v => !v.reported)
                      .Select(v => v.variation);
            }

            protected static IEnumerable<(bool logged, bool reported, object[] variation)> GetMessageVariations()
            {
                foreach (var verbosityLevel in new int[] { 0, 1, 2, 3, 4 })
                {
                    // Every information message is logged, but not reported, regardless of verbosity level.
                    yield return (logged: true, reported: false, new object[] { LogLevel.Information, verbosityLevel });

                    // Every warning and error message is logged and reported, regardless of verbosity level.
                    yield return (logged: true, reported: true, new object[] { LogLevel.Warning, verbosityLevel });
                    yield return (logged: true, reported: true, new object[] { LogLevel.Error, verbosityLevel });

                    // For all others log levels, we are logging, but not reporting, only if verbosity level is above 2
                    foreach (var logLevel in new LogLevel[] { LogLevel.Minimal, LogLevel.Verbose, LogLevel.Debug })
                    {
                        if (verbosityLevel > 2)
                        {
                            yield return (logged: true, reported: false, new object[] { logLevel, verbosityLevel });
                        }
                        else
                        {
                            yield return (logged: false, reported: false, new object[] { logLevel, verbosityLevel });
                        }
                    }
                }
            }
        }
    }
}
