// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Logging;

[Export(typeof(ILoggerFactory)), Shared]
[method: ImportingConstructor]
internal partial class RemoteLoggerFactory() : ILoggerFactory
{
    private static TraceSource? s_traceSource;

    internal static TraceSource Initialize(IServiceProvider hostProvidedServices)
    {
        s_traceSource ??= (TraceSource)hostProvidedServices.GetService(typeof(TraceSource));
        return s_traceSource;
    }

    public void AddLoggerProvider(ILoggerProvider provider)
    {
        throw new NotImplementedException();
    }

    public ILogger GetOrCreateLogger(string categoryName)
    {
        return new Logger(categoryName);
    }

    public void Dispose()
    {
    }
}
