// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class RazorLanguageServer
{
    /// <summary>
    /// Very small wrapper around the <see cref="ILoggerFactory"/> to add a prefix to the category name, so we can tell in the logs
    /// whether things are coming from the VS side, the LSP side, of our code. This is only temporary and will be removed when we move
    /// to cohosting as there will only be one side.
    /// </summary>
    private sealed class LoggerFactoryWrapper(ILoggerFactory loggerFactory) : ILoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        public void AddLoggerProvider(ILoggerProvider provider)
        {
            _loggerFactory.AddLoggerProvider(provider);
        }

        public ILogger GetOrCreateLogger(string categoryName)
        {
            // Adding [LSP] to the start to identify the LSP server, as some of our services exist in the server, and in VS
            // It looks weird because the category is surround with square brackets, so this ends up being [LSP][Category]
            return _loggerFactory.GetOrCreateLogger($"LSP][{categoryName}");
        }
    }
}
