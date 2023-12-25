// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class RazorLanguageServer
{
    private class RazorLoggerProviderWrapper : IRazorLoggerProvider
    {
        private ILoggerProvider _provider;

        public RazorLoggerProviderWrapper(ILoggerProvider provider)
        {
            _provider = provider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _provider.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
