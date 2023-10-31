// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Logging;

public static class LoggerBuilderExtensions
{
    public static ILoggingBuilder AddTestOutput(this ILoggingBuilder builder, ITestOutputHelper testOutput)
        => builder
            .AddProvider(new TestOutputLoggerProvider(testOutput))
            .SetMinimumLevel(LogLevel.Trace);
}
