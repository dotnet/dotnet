// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

[Export(typeof(IRazorLoggerProvider))]
[method: ImportingConstructor]
internal partial class MemoryLoggerProvider() : IRazorLoggerProvider
{
    // How many messages will the buffer contain
    private const int BufferSize = 5000;
    private readonly Buffer _buffer = new(BufferSize);

    public ILogger CreateLogger(string categoryName)
        => new Logger(_buffer, categoryName);

    public void Dispose()
    {
    }
}
