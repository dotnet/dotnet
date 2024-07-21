// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Logging;

internal class TestOutputLoggerProvider(ITestOutputHelper output, LogLevel logLevel = LogLevel.Trace) : ILoggerProvider
{
    private readonly ITestOutputHelper _output = output;
    private readonly LogLevel _logLevel = logLevel;

    public ITestOutputHelper Output => _output;

    public ILogger CreateLogger(string categoryName)
        => new TestOutputLogger(this, categoryName, _logLevel);
}
