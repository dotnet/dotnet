// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LoggerProvider(LogLevel logLevel, LspLogger mainLspLogger) : IRazorLoggerProvider
{
    private readonly LogLevel _logLevel = logLevel;
    private readonly LspLogger _mainLspLogger = mainLspLogger;

    public ILogger CreateLogger(string categoryName)
    {
        // The main LSP logger is the only one the server will have initialized with a client connection, so
        // we have to make sure we pass it along.
        return new LspLogger(categoryName, _logLevel, _mainLspLogger.GetClientConnection());
    }

    public void Dispose()
    {
    }
}
