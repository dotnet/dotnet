// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat.Utility
{
    internal class RemappedLevelLogger : LoggerBase
    {
        private readonly ILogger _logger;
        private readonly Mapping _mapping;

        public RemappedLevelLogger(ILogger inner, Mapping mapping)
        {
            _logger = inner;
            _mapping = mapping;
        }

        private LogLevel GetMappedLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => _mapping.Debug,
                LogLevel.Verbose => _mapping.Verbose,
                LogLevel.Information => _mapping.Information,
                LogLevel.Minimal => _mapping.Minimal,
                LogLevel.Warning => _mapping.Warning,
                LogLevel.Error => _mapping.Error,
                _ => throw new System.ArgumentOutOfRangeException(nameof(level), level, "Unknown log level")
            };
        }

        public override void Log(ILogMessage message)
        {
            message.Level = GetMappedLevel(message.Level);
            _logger.Log(message);
        }

        public override Task LogAsync(ILogMessage message)
        {
            message.Level = GetMappedLevel(message.Level);
            return _logger.LogAsync(message);
        }

        internal record Mapping
        {
            public LogLevel Debug { get; init; } = LogLevel.Debug;
            public LogLevel Verbose { get; init; } = LogLevel.Verbose;
            public LogLevel Information { get; init; } = LogLevel.Information;
            public LogLevel Minimal { get; init; } = LogLevel.Minimal;
            public LogLevel Warning { get; init; } = LogLevel.Warning;
            public LogLevel Error { get; init; } = LogLevel.Error;
        }
    }
}
