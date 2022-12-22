// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace NuGet.Protocol
{
    internal class RegistrationInfo
    {
        public string Id { get; set; }
        public bool IncludePrerelease { get; set; }
        public IList<PackageInfo> Packages { get; private set; }

        public RegistrationInfo()
        {
            Packages = new List<PackageInfo>();
        }

        public void Add(PackageInfo packageInfo)
        {
            packageInfo.Registration = this;
            Packages.Add(packageInfo);
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} Packages: {1}", Id, Packages.Count);
        }
    }
}
