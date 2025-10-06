// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.Logging;

[ExportLoggerProvider]
internal partial class MemoryLoggerProvider : ILoggerProvider
{
    // How many messages will the buffer contain
    private const int BufferSize = 5000;
    private readonly Buffer _buffer = new(BufferSize);

    public ILogger CreateLogger(string categoryName)
        => new Logger(_buffer, categoryName);
}
