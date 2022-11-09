// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    internal class DependencyInfo
    {
        public string Id { get; set; }
        public VersionRange Range { get; set; }
        public RegistrationInfo RegistrationInfo { get; set; }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} {1}", Id, Range);
        }
    }
}
