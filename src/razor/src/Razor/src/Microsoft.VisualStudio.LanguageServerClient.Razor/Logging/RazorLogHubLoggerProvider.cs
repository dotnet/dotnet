// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Editor.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

[Export(typeof(ILoggerProvider))]
internal sealed class RazorLogHubLoggerProvider : ILoggerProvider
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
