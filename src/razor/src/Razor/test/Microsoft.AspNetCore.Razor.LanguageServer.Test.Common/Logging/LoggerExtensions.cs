// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;

#pragma warning disable CA2254 // Template should be a static expression

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common.Logging;

internal static class LoggerExtensions
{
    private class TestLspLogger : ILspLogger
    {
        private readonly ILogger _logger;
        private Stack<IDisposable?>? _scopeStack;
        private readonly object _gate = new();

        public TestLspLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogError(string? message, params object?[] @params)
            => _logger.LogError(message, @params);

        public void LogException(Exception exception, string? message = null, params object?[] @params)
            => _logger.LogError(exception, message, @params);

        public void LogInformation(string? message, params object?[] @params)
            => _logger.LogInformation(message, @params);

        public void LogWarning(string? message, params object?[] @params)
            => _logger.LogWarning(message, @params);

        public void LogDebug(string message, params object[] @params)
            => _logger.LogDebug(message, @params);

        public void LogStartContext(string message, params object?[] @params)
        {
            lock (_gate)
            {
                _scopeStack ??= new();
                _scopeStack.Push(_logger.BeginScope(message, @params));
            }
        }

        public void LogEndContext(string message, params object[] @params)
        {
            lock (_gate)
            {
                var scope = _scopeStack?.Pop();
                scope?.Dispose();
            }
        }
    }
}
