// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.Common;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public class End : OutputConsoleLoggerTests
        {
            public End(GlobalServiceProvider sp)
                : base(sp)
            {
                _outputConsole.Reset();
            }

            [Fact]
            public void Writes_message_that_it_is_finished()
            {
                _outputConsoleLogger.End();
                _outputConsole.Verify(oc => oc.WriteLineAsync(Resources.Finished));
            }

            [Fact]
            public void Writes_empty_line()
            {
                _outputConsoleLogger.End();
                _outputConsole.Verify(oc => oc.WriteLineAsync(string.Empty));
            }

            [Fact]
            public void BringToFrontIfSettingsPermitAsync_WhenAnyCallsToReportError_IsCalled()
            {
                // ReportError will always add error entries regardless of log level.
                LogLevel irrelevantLogLevel = LogLevel.Information;
                _outputConsoleLogger.ReportError(new LogMessage(irrelevantLogLevel, "message"));
                _outputConsoleLogger.End();
                _errorList.Verify(el => el.BringToFrontIfSettingsPermitAsync());
            }

            [Fact]
            public void BringToFrontIfSettingsPermitAsync_WhenNoCallsToReportError_IsCalledOnce()
            {
                _outputConsoleLogger.End();
                _errorList.Verify(el => el.BringToFrontIfSettingsPermitAsync(), Times.Once);
            }
        }
    }
}
