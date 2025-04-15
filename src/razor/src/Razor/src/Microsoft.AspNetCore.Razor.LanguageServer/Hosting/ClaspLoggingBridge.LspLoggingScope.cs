// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal partial class ClaspLoggingBridge
{
    private class LspLoggingScope : IDisposable
    {
        private string _context;
        private ILogger _logger;

        public LspLoggingScope(string context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            // This is a special log message formatted so that the LogHub logger can detect it, and trigger the right trace event
            _logger.LogInformation($"{LogStartContextMarker} {_context}");
        }

        public void Dispose()
        {
            // This is a special log message formatted so that the LogHub logger can detect it, and trigger the right trace event
            _logger.LogInformation($"{LogEndContextMarker} {_context}");
        }
    }
}
