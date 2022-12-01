// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Test;

internal class TestLoggerProvider : HTMLCSharpLanguageServerLogHubLoggerProvider
{
    public TestLoggerProvider()
        : base(new HTMLCSharpLanguageServerLogHubLoggerProviderFactory(
            new TestRazorLogHubTraceProvider()))
    {
    }

    public override ILogger CreateLogger(string categoryName) => TestLogger.Instance;

    private class TestRazorLogHubTraceProvider : RazorLogHubTraceProvider
    {
        public override Task<TraceSource?> InitializeTraceAsync(string logIdentifier, int logHubSessionId, CancellationToken cancellationToken)
            => Task.FromResult<TraceSource?>(new TraceSource("test"));
    }
}
