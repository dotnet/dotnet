// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    internal class DependencyInfo
    {
        public required string Id { get; init; }
        public required VersionRange Range { get; init; }

        /// <summary>
        /// NULL_INC: Set externally by the resolver after construction.
        /// </summary>
        public RegistrationInfo RegistrationInfo { get; set; } = null!;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", Id, Range);
        }
    }
}
