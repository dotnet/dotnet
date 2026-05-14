// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public class Start : OutputConsoleLoggerTests
        {
            public Start(GlobalServiceProvider sp)
                : base(sp)
            {
                _outputConsole.Reset();
                _outputConsoleLogger.Start();
            }

            [Fact]
            public void Activates_output_console()
            {
                _outputConsole.Verify(oc => oc.ActivateAsync());
            }

            [Fact]
            public void Clears_output_console()
            {
                _outputConsole.Verify(oc => oc.ClearAsync());
            }

            [Fact]
            public void Clears_error_list()
            {
                _errorList.Verify(el => el.ClearNuGetEntries());
            }
        }
    }
}
