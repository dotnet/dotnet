// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using NuGet.Common;
using System.Threading.Tasks;
using MSBuild = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class SleetLogger : LoggerBase
    {
        private MSBuild.TaskLoggingHelper _log;

        private readonly LogLevel _logLevel;

        public SleetLogger(MSBuild.TaskLoggingHelper log, LogLevel logLevel = default(LogLevel))
        {
            _log = log;
            _logLevel = logLevel;
        }

        public override void Log(ILogMessage message)
        {
            Log(message, _logLevel);
        }

        public override Task LogAsync(ILogMessage message)
        {
            return Task.Run(() => Log(message, _logLevel));
        }

        private void Log(ILogMessage message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    _log.LogError(message.Message);
                    break;

                case LogLevel.Warning:
                    _log.LogWarning(message.Message);
                    break;

                case LogLevel.Minimal:
                    _log.LogMessage(MessageImportance.Low, message.Message);
                    break;

                case LogLevel.Information:
                    _log.LogMessage(MessageImportance.Normal, message.Message);
                    break;

                case LogLevel.Debug:
                case LogLevel.Verbose:
                default:
                    _log.LogMessage(MessageImportance.High, message.Message);
                    break;
            }

            return;
        }
    }
}
