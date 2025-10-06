// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System;
using Microsoft.SymbolStore;

namespace Microsoft.DotNet.Internal.SymbolHelper;

internal class ScopedTracer : ITracer
{
    private readonly ITracer _logger;
    private readonly string _operationIdentifier;

    private string? _subScope;

    public ScopedTracer(ITracer logger, string operationName)
    {
        _logger = logger;
        _operationIdentifier = $"{operationName}/{Guid.NewGuid()}";
    }

    private string ScopedMessage(string message) => _subScope is null
                ? $"[{_operationIdentifier}] {message}"
                : $"[{_operationIdentifier}/{_subScope}] {message}";

    private void LogToMethod(Action<string, object[]> logMethod, string format, object[] arguments) => logMethod(ScopedMessage(format), arguments);

    private void LogToMethod(Action<string> logMethod, string message) => logMethod(ScopedMessage(message));


    public void Error(string message) => LogToMethod(_logger.Error, message);
    public void Error(string format, params object[] arguments) => LogToMethod(_logger.Error, format, arguments);
    public void Information(string message) => LogToMethod(_logger.Information, message);
    public void Information(string format, params object[] arguments) => LogToMethod(_logger.Information, format, arguments);
    public void Verbose(string message) => LogToMethod(_logger.Verbose, message);
    public void Verbose(string format, params object[] arguments) => LogToMethod(_logger.Verbose, format, arguments);
    public void Warning(string message) => LogToMethod(_logger.Warning, message);
    public void Warning(string format, params object[] arguments) => LogToMethod(_logger.Warning, format, arguments);
    public void WriteLine(string message) => LogToMethod(_logger.WriteLine, message);
    public void WriteLine(string format, params object[] arguments) => LogToMethod(_logger.WriteLine, format, arguments);
    public IDisposable AddSubScope(string scope) => new TokenScope(this, scope);

    private sealed class TokenScope : IDisposable
    {
        private readonly ScopedTracer _tracer;
        private readonly string? _priorScope;

        public TokenScope(ScopedTracer tracer, string? newScope)
        {
            _tracer = tracer;
            _priorScope = _tracer._subScope;
            _tracer._subScope = _priorScope is null ? newScope : $"{_priorScope}/{newScope}";
        }

        public void Dispose() => _tracer._subScope = _priorScope;
    }
}

internal class ScopedTracerFactory
{
    private readonly ITracer _logger;

    public ScopedTracerFactory(ITracer logger) => _logger = logger;

    public ScopedTracer CreateTracer(string operationName) => new ScopedTracer(_logger, operationName);
}
