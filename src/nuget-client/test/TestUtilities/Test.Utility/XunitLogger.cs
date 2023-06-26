// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NuGet.Common;
using Xunit.Abstractions;

namespace Test.Utility
{
    /// <summary>
    /// ILogger -> Xunit
    /// </summary>
    public class XunitLogger : LoggerBase
    {
        public XunitLogger(ITestOutputHelper output)
        {
            Output = output;
            VerbosityLevel = LogLevel.Information;
        }

        /// <summary>
        /// Xunit output
        /// </summary>
        public ITestOutputHelper Output { get; }

        public override void Log(ILogMessage message)
        {
            var level = message.Level.ToString().ToLowerInvariant();
            level = level.Substring(0, Math.Min(level.Length, 4));

            Output.WriteLine($"[test] {level}: {message.Message}");
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);

            return Task.FromResult(true);
        }
    }
}
