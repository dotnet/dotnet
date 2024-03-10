// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat
{
    internal static class ExitCodes
    {
        // Exit code use when the command line arguments are parsed successfully and the command is run successfully
        internal const int Success = 0;

        // Exit code used when te command line arguments are not parsed successfully.
        internal const int InvalidArguments = 1;

        // Exit code used when the command line arguments are parsed successfully bu the command fails to run successfully.
        internal const int Error = 2;
    }
}
