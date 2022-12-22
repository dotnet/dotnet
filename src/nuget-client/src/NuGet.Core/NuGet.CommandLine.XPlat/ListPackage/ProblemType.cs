// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat.ListPackage
{
    internal enum ProblemType
    {
        Warning,
        Error // Any report problem with this type make application to return 1 instead of 0, for example if asset file is missing for 1 of the projects.
    }
}
