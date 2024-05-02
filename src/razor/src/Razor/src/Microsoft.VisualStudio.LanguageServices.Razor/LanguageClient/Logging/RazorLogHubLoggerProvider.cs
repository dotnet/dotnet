// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Logging;

[ExportLoggerProvider]
[method: ImportingConstructor]
internal sealed class RazorLogHubLoggerProvider(RazorLogHubTraceProvider traceProvider) : ILoggerProvider
{
    private readonly RazorLogHubTraceProvider _traceProvider = traceProvider;

    public ILogger CreateLogger(string categoryName)
    {
        return new RazorLogHubLogger(categoryName, _traceProvider);
    }

    public void Dispose()
    {
    }
}
