// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using NuGet.CommandLine.XPlat;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    internal class TestCommandOutputLogger : CommandOutputLogger
    {
        private readonly bool _observeLogLevel;

        public TestLogger Logger { get; set; }

        public TestCommandOutputLogger(ITestOutputHelper testOutputHelper, bool observeLogLevel = false)
            : base(LogLevel.Debug)
        {
            _observeLogLevel = observeLogLevel;

            Logger = new TestLogger(testOutputHelper);
        }

        protected override void LogInternal(LogLevel logLevel, string message)
        {
            if (_observeLogLevel && logLevel < VerbosityLevel)
            {
                return;
            }

            switch (logLevel)
            {
                case LogLevel.Debug:
                    Logger.LogDebug(message);
                    break;
                case LogLevel.Error:
                    Logger.LogError(message);
                    break;
                case LogLevel.Information:
                    Logger.LogInformation(message);
                    break;
                case LogLevel.Minimal:
                    Logger.LogMinimal(message);
                    break;
                case LogLevel.Verbose:
                    Logger.LogVerbose(message);
                    break;
                case LogLevel.Warning:
                    Logger.LogWarning(message);
                    break;
                default:
                    Logger.LogDebug(message);
                    break;
            }
        }

        public override void LogMinimal(string data, ConsoleColor color)
        {
            LogInternal(LogLevel.Minimal, data);
        }

        public ConcurrentQueue<string> Messages
        {
            get
            {
                return Logger.Messages;
            }
        }

        public ConcurrentQueue<string> WarningMessages => Logger.WarningMessages;

        public ConcurrentQueue<string> ErrorMessages
        {
            get
            {
                return Logger.ErrorMessages;
            }
        }

        public ConcurrentQueue<string> VerboseMessages
        {
            get
            {
                return Logger.VerboseMessages;
            }
        }

        public int Errors
        {
            get
            {
                return Logger.Errors;
            }
        }

        public int Warnings
        {
            get
            {
                return Logger.Warnings;
            }
        }

        public string ShowMessages()
        {
            return string.Join(Environment.NewLine, Messages);
        }

        public string ShowErrors()
        {
            return string.Join(Environment.NewLine, ErrorMessages);
        }

        public string ShowVerboseMessages()
        {
            return string.Join(Environment.NewLine, VerboseMessages);
        }
    }
}
