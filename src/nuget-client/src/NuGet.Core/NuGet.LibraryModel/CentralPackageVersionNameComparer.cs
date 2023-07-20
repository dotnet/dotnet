// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.LibraryModel
{
    public sealed class CentralPackageVersionNameComparer : IEqualityComparer<CentralPackageVersion>
    {
        /// <summary>
        /// Returns a singleton instance for the <see cref="CentralPackageVersionNameComparer"/>.
        /// </summary>
        public static CentralPackageVersionNameComparer Default { get; } = new();

        /// <summary>
        /// Get a singleton instance only through the <see cref="CentralPackageVersionNameComparer.Default"/>.
        /// </summary>
        private CentralPackageVersionNameComparer()
        {
        }

        public bool Equals(CentralPackageVersion x, CentralPackageVersion y)
        {
            return string.Equals(x?.Name, y?.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(CentralPackageVersion obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
        }
    }
}

