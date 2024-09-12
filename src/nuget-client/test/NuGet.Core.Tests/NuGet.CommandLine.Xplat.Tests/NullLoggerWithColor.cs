// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.CommandLine.XPlat;
using NuGet.Common;

namespace NuGet.CommandLine.Xplat.Tests
{
    internal class NullLoggerWithColor : NullLogger, ILoggerWithColor
    {
        public static new NullLoggerWithColor Instance { get; } = new NullLoggerWithColor();

        public static NullLoggerWithColor GetInstance() { return Instance; }

        public void LogMinimal(string data, ConsoleColor color)
        {
        }
    }
}
