// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
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
                _outputConsoleLogger.End();
            }

            [Fact]
            public void Writes_message_that_it_is_finished()
            {
                _outputConsole.Verify(oc => oc.WriteLineAsync(Resources.Finished));
            }

            [Fact]
            public void Writes_empty_line()
            {
                _outputConsole.Verify(oc => oc.WriteLineAsync(string.Empty));
            }

            [Fact]
            public void Gives_error_list_focus()
            {
                _errorList.Verify(el => el.BringToFrontIfSettingsPermitAsync());
            }
        }
    }
}
