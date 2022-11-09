// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Configuration
{
    [Obsolete("https://github.com/NuGet/Home/issues/10098")]
    public sealed class PackageSourceUpdateOptions
    {
        public readonly static PackageSourceUpdateOptions Default = new PackageSourceUpdateOptions(true, true);

        public bool UpdateCredentials { get; }
        public bool UpdateEnabled { get; }

        public PackageSourceUpdateOptions(bool updateCredentials, bool updateEnabled)
        {
            UpdateCredentials = updateCredentials;
            UpdateEnabled = updateEnabled;
        }
    }
}
