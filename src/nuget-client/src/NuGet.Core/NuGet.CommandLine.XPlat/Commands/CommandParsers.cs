// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal static class CommandParsers
    {
        public static void Register(CommandLineApplication app, Func<ILogger> getLogger)
        {
            AddVerbParser.Register(app, getLogger);
            DisableVerbParser.Register(app, getLogger);
            EnableVerbParser.Register(app, getLogger);
            ListVerbParser.Register(app, getLogger);
            RemoveVerbParser.Register(app, getLogger);
            UpdateVerbParser.Register(app, getLogger);
        }
    }
}
