// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    internal class VsPackageMetadataComparer : IEqualityComparer<IVsPackageMetadata>
    {
        public static VsPackageMetadataComparer Instance { get; } = new();

        public bool Equals(IVsPackageMetadata x, IVsPackageMetadata y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            return StringComparer.OrdinalIgnoreCase.Equals(x.VersionString, y.VersionString)
                   && StringComparer.OrdinalIgnoreCase.Equals(x.Id, y.Id);
        }

        public int GetHashCode(IVsPackageMetadata obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Id) * 397
                   ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.VersionString);
        }
    }
}
