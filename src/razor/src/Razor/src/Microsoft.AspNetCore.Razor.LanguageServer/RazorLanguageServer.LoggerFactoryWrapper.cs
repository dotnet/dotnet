// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class RazorLanguageServer
{
    private class LoggerFactoryWrapper : ILoggerFactory, IRazorLoggerFactory
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

        public void AddProvider(ILoggerProvider provider)
        {
            _loggerFactory.AddLoggerProvider(new RazorLoggerProviderWrapper(provider));
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
