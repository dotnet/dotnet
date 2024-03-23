// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Logging;

[Export(typeof(IRazorLoggerFactory)), Shared]
[method: ImportingConstructor]
internal partial class RemoteLoggerFactory() : IRazorLoggerFactory
{
    private static TraceSource? s_traceSource;

    internal static void Initialize(TraceSource traceSource)
    {
        s_traceSource ??= traceSource;
    }

    public void AddLoggerProvider(IRazorLoggerProvider provider)
    {
        throw new System.NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(categoryName);
    }

    public void Dispose()
    {
    }
}
