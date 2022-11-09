// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public class ReportError : LogAndReportErrorTests
        {
            [Theory]
            [MemberData(nameof(GetAllMessageVariations))]
            public void Adds_entry_to_error_list(LogLevel logLevel, int verbosityLevel)
            {
                VerifyThatEntryToErrorListIsAdded(_outputConsoleLogger.ReportError, logLevel, verbosityLevel);
            }

            public static IEnumerable<object[]> GetAllMessageVariations()
            {
                return GetMessageVariations()
                      .Select(v => v.variation);
            }
        }
    }
}
