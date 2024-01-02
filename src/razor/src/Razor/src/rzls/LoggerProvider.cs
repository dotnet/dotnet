// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LoggerProvider(LogLevel logLevel, IClientConnection clientConnection) : IRazorLoggerProvider
{
    private readonly LogLevel _logLevel = logLevel;
    private readonly IClientConnection _clientConnection = clientConnection;

    public ILogger CreateLogger(string categoryName)
    {
        return new LspLogger(categoryName, _logLevel, _clientConnection);
    }

    public void Dispose()
    {
    }
}
