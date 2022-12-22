// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Moq;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public class Dispose : OutputConsoleLoggerTests
        {
            public Dispose()
            {
                _errorList.Reset();
                _outputConsoleLogger.Dispose();
            }

            [Fact]
            public void Disposes_error_list()
            {
                _errorList.Verify(el => el.Dispose());
            }
        }
    }
}
