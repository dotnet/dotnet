// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

[Shared]
[Export(typeof(IRazorLoggerProvider))]
internal sealed class RazorLogHubLoggerProvider : IRazorLoggerProvider
{
    private readonly RazorLogHubTraceProvider _traceProvider;

    [ImportingConstructor]
    public RazorLogHubLoggerProvider(RazorLogHubTraceProvider traceProvider)
    {
        _traceProvider = traceProvider;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new RazorLogHubLogger(categoryName, _traceProvider);
    }

    public void Dispose()
    {
    }
}
