// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.CommandLine;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat.Commands;

internal static class CommonOptions
{
    internal static Option<VerbosityEnum?> GetVerbosityOption()
    {
        var verbosityOption = new Option<VerbosityEnum?>("--verbosity", "-v")
        {
            Description = Strings.Verbosity_Description
        };
        return verbosityOption;
    }

    internal static LogLevel ToLogLevel(this VerbosityEnum verbosity)
    {
        return verbosity switch
        {
            VerbosityEnum.q => LogLevel.Warning,
            VerbosityEnum.quiet => LogLevel.Warning,
            VerbosityEnum.m => LogLevel.Minimal,
            VerbosityEnum.minimal => LogLevel.Minimal,
            VerbosityEnum.n => LogLevel.Information,
            VerbosityEnum.normal => LogLevel.Information,
            VerbosityEnum.d => LogLevel.Verbose,
            VerbosityEnum.detailed => LogLevel.Verbose,
            VerbosityEnum.diag => LogLevel.Debug,
            VerbosityEnum.diagnostic => LogLevel.Debug,
            _ => throw new ArgumentOutOfRangeException(nameof(verbosity), verbosity, null)
        };
    }
}
