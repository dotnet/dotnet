// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Verbosity level of restore operation logger.
    /// </summary>
    internal enum VerbosityLevel
    {
        Quiet = 0,
        Minimal = 1,
        Normal = 2,
        Detailed = 3,
        Diagnostic = 4
    };
}
