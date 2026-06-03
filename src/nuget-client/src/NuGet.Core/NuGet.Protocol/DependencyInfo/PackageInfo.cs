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
        /// <summary>
        /// NULL_INC: Set by <see cref="RegistrationInfo.Add"/> after construction.
        /// </summary>
        public RegistrationInfo Registration { get; set; } = null!;
        public bool Listed { get; init; }
        public required NuGetVersion Version { get; init; }
        public required Uri PackageContent { get; init; }
        public IList<DependencyInfo> Dependencies { get; } = new List<DependencyInfo>();

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", Registration.Id, Version.ToNormalizedString());
        }
    }
}
