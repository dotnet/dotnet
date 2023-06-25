// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// Call legacy Log* methods from LogAsync/Log.
    /// This is for legacy ILogger implementations,
    /// new loggers should use LoggerBase.
    /// </summary>
    public abstract class LegacyLoggerAdapter : ILogger
    {
        public void Log(LogLevel level, string data)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    {
                        LogDebug(data);
                        break;
                    }

                case LogLevel.Error:
                    {
                        LogError(data);
                        break;
                    }

                case LogLevel.Information:
                    {
                        LogInformation(data);
                        break;
                    }

                case LogLevel.Minimal:
                    {
                        LogMinimal(data);
                        break;
                    }

                case LogLevel.Verbose:
                    {
                        LogVerbose(data);
                        break;
                    }

                case LogLevel.Warning:
                    {
                        LogWarning(data);
                        break;
                    }
            }
        }

        public Task LogAsync(LogLevel level, string data)
        {
            Log(level, data);

            return Task.CompletedTask;
        }

        public virtual void Log(ILogMessage message)
        {
            Log(message.Level, message.Message);
        }

        public virtual async Task LogAsync(ILogMessage message)
        {
            await LogAsync(message.Level, message.Message);
        }

        public abstract void LogDebug(string data);

        public abstract void LogVerbose(string data);

        public abstract void LogInformation(string data);

        public abstract void LogMinimal(string data);

        public abstract void LogWarning(string data);

        public abstract void LogError(string data);

        public abstract void LogInformationSummary(string data);

    }
}
