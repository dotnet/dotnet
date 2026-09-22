// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update;

internal sealed class PackageUpdateException : Exception
{
    public PackageUpdateException(string message)
        : base(message)
    {
    }
}
