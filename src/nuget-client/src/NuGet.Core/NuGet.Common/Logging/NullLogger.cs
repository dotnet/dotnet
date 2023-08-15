// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NuGet.Common
{
    public class NullLogger : LoggerBase
    {
        private static ILogger? _instance;

        public static ILogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NullLogger();
                }

                return _instance;
            }
        }

        public override void Log(ILogMessage message) { }

        public override void Log(LogLevel level, string data) { }

        public override Task LogAsync(ILogMessage message) { return Task.CompletedTask; }

        public override Task LogAsync(LogLevel level, string data) { return Task.CompletedTask; }

    }
}
