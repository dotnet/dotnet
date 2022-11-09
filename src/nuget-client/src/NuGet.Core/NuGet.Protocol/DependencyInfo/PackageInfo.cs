// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    internal class PackageInfo
    {
        public RegistrationInfo Registration { get; set; }
        public bool Listed { get; set; }
        public NuGetVersion Version { get; set; }
        public Uri PackageContent { get; set; }
        public IList<DependencyInfo> Dependencies { get; private set; }

        public PackageInfo()
        {
            Dependencies = new List<DependencyInfo>();
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} {1}", Registration.Id, Version.ToNormalizedString());
        }
    }
}
