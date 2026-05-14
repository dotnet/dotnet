// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.Common
{
    public abstract class LoggerBase : ILogger
    {
        public LogLevel VerbosityLevel { get; set; } = LogLevel.Debug;

        public LoggerBase()
        {
        }

        public LoggerBase(LogLevel verbosityLevel)
        {
            VerbosityLevel = verbosityLevel;
        }

        public abstract void Log(ILogMessage message);

        public abstract Task LogAsync(ILogMessage message);

        public virtual void Log(LogLevel level, string data)
        {
            if (DisplayMessage(level))
            {
                Log(new LogMessage(level, data));
            }
        }

        public virtual Task LogAsync(LogLevel level, string data)
        {
            if (DisplayMessage(level))
            {
                return LogAsync(new LogMessage(level, data));
            }

            return TaskResult.True;
        }

        public virtual void LogDebug(string data)
        {
            Log(LogLevel.Debug, data);
        }

        public virtual void LogError(string data)
        {
            Log(LogLevel.Error, data);
        }

        public virtual void LogInformation(string data)
        {
            Log(LogLevel.Information, data);
        }

        public virtual void LogInformationSummary(string data)
        {
            Log(LogLevel.Information, data);
        }

        public virtual void LogMinimal(string data)
        {
            Log(LogLevel.Minimal, data);
        }

        public virtual void LogVerbose(string data)
        {
            Log(LogLevel.Verbose, data);
        }

        public virtual void LogWarning(string data)
        {
            Log(LogLevel.Warning, data);
        }

        /// <summary>
        /// True if the message meets the verbosity level.
        /// </summary>
        protected virtual bool DisplayMessage(LogLevel messageLevel)
        {
            return (messageLevel >= VerbosityLevel);
        }

        /// <summary>
        /// True if the message is an error or warning.
        /// </summary>
        protected virtual bool CollectMessage(LogLevel messageLevel)
        {
            return (messageLevel >= LogLevel.Warning);
        }
    }
}
