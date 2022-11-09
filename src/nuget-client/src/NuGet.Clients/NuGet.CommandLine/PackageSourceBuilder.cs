// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Configuration;

namespace NuGet.CommandLine
{
    internal static class PackageSourceBuilder
    {
        internal static PackageSourceProvider CreateSourceProvider(ISettings settings)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return new PackageSourceProvider(settings, enablePackageSourcesChangedEvent: false);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
