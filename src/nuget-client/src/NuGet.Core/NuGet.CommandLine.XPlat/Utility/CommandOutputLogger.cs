// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    /// <summary>
    /// Logger to print formatted command output.
    /// </summary>
    internal class CommandOutputLogger : LoggerBase, ILogger
    {
        public CommandOutputLogger(LogLevel logLevel)
        {
            VerbosityLevel = logLevel;
        }

        public override void LogDebug(string data)
        {
            LogInternal(LogLevel.Debug, data);
        }

        public override void LogError(string data)
        {
            LogInternal(LogLevel.Error, data);
        }
        public override void LogInformation(string data)
        {
            LogInternal(LogLevel.Information, data);
        }

        public override void LogMinimal(string data)
        {
            LogInternal(LogLevel.Minimal, data);
        }

        public override void LogVerbose(string data)
        {
            LogInternal(LogLevel.Verbose, data);
        }

        public override void LogWarning(string data)
        {
            LogInternal(LogLevel.Warning, data);
        }

        public override void LogInformationSummary(string data)
        {
            if (DisplayMessage(LogLevel.Information))
            {
                Console.WriteLine(data);
            }
        }

        internal bool HidePrefixForInfoAndMinimal { get; set; }

        protected virtual void LogInternal(LogLevel logLevel, string message)
        {
            if (!DisplayMessage(logLevel))
            {
                return;
            }

            var caption = string.Empty;

            switch (logLevel)
            {
                case LogLevel.Debug:
                    caption = "debug: ";
                    break;
                case LogLevel.Verbose:
                    caption = "trace: ";
                    break;
                case LogLevel.Information:
                    caption = HidePrefixForInfoAndMinimal ? null : "info : ";
                    break;
                case LogLevel.Minimal:
                    caption = HidePrefixForInfoAndMinimal ? null : "log  : ";
                    break;
                case LogLevel.Warning:
                    caption = "warn : ";
                    break;
                case LogLevel.Error:
                    caption = "error: ";
                    break;
            }

            if (message.IndexOf('\n') >= 0)
            {
                Console.Write(PrefixAllLines(caption, message));
            }
            else
            {
                Console.WriteLine($"{caption}{message}");
            }
        }

        private static string PrefixAllLines(string caption, string message)
        {
            // handle messages with multiple lines
            var builder = new StringBuilder();
            using (var reader = new StringReader(message))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    builder.Append(caption);
                    builder.AppendLine(line);
                }
            }

            return builder.ToString();
        }

        public override void Log(ILogMessage message)
        {
            LogInternal(message.Level, message.FormatWithCode());
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);

            return Task.CompletedTask;
        }
    }
}
