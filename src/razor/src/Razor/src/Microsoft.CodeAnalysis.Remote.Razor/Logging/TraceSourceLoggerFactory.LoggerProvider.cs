// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Logging;

internal sealed partial class TraceSourceLoggerFactory
{
    private sealed class LoggerProvider(TraceSource traceSource) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
            => new Logger(traceSource, categoryName);
    }
}
