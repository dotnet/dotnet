// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Logging;

internal class TestOutputLoggerProvider(ITestOutputHelper output) : ILoggerProvider
{
    private ITestOutputHelper? _output = output;

    public ITestOutputHelper? TestOutputHelper => _output;

    public ILogger CreateLogger(string categoryName)
        => new TestOutputLogger(this, categoryName);

    public void Dispose()
    {
    }

    internal void SetTestOutputHelper(ITestOutputHelper? testOutputHelper)
    {
        _output = testOutputHelper;
    }
}
