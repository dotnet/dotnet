// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Commands;

namespace NuGet.CommandLine
{
    [Serializable]
    public sealed class ExitCodeException : CommandException
    {
        public ExitCodeException(int exitCode)
        {
            ExitCode = exitCode;
        }

        public int ExitCode { get; }
    }
}
