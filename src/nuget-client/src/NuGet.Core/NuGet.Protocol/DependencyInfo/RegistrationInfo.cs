// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;

namespace NuGet.Protocol
{
    internal class RegistrationInfo
    {
        public required string Id { get; init; }
        public required bool IncludePrerelease { get; init; }
        public IList<PackageInfo> Packages { get; } = new List<PackageInfo>();

        public void Add(PackageInfo packageInfo)
        {
            packageInfo.Registration = this;
            Packages.Add(packageInfo);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} Packages: {1}", Id, Packages.Count);
        }
    }
}
