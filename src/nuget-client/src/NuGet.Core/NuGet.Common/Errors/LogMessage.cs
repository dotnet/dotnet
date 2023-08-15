// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    /// <summary>
    /// Basic log message.
    /// </summary>
    public class LogMessage : ILogMessage
    {
        public LogLevel Level { get; set; }
        public WarningLevel WarningLevel { get; set; } = WarningLevel.Severe; //setting default to Severe as 0 implies show no warnings
        public NuGetLogCode Code { get; set; }
        public string Message { get; set; }
        public string? ProjectPath { get; set; }
        public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;

        public LogMessage(LogLevel level, string message, NuGetLogCode code)
            : this(level, message)
        {
            Code = code;
        }

        public LogMessage(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }

        public static LogMessage CreateError(NuGetLogCode code, string message)
        {
            return new LogMessage(LogLevel.Error, message, code);
        }

        public static LogMessage CreateWarning(NuGetLogCode code, string message)
        {
            return new LogMessage(LogLevel.Warning, message, code);
        }

        public static LogMessage Create(LogLevel level, string message)
        {
            return new LogMessage(level, message);
        }
    }
}
