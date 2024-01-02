// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class RazorLanguageServer
{
    /// <summary>
    /// Very small wrapper around the <see cref="IRazorLoggerFactory"/> to add a prefix to the category name, so we can tell in the logs
    /// whether things are coming from the VS side, the LSP side, of our code. This is only temporary and will be removed when we move
    /// to cohosting as there will only be one side.
    /// </summary>
    private class LoggerFactoryWrapper : IRazorLoggerFactory
    {
        private IRazorLoggerFactory _loggerFactory;

        public LoggerFactoryWrapper(IRazorLoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void AddLoggerProvider(IRazorLoggerProvider provider)
        {
            _loggerFactory.AddLoggerProvider(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            // Adding [LSP] to the start to identify the LSP server, as some of our services exist in the server, and in VS
            // It looks weird because the category is surround with square brackets, so this ends up being [LSP][Category]
            return _loggerFactory.CreateLogger($"LSP][{categoryName}");
        }

        public void Dispose()
        {
            _loggerFactory.Dispose();
        }
    }
}
