// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LoggerProvider(LogLevelProvider logLevelProvider, IClientConnection clientConnection) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new LspLogger(categoryName, logLevelProvider, clientConnection);
    }
}
