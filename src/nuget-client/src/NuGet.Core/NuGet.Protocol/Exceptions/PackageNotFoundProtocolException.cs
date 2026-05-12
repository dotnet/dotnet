// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    /// <summary>
    /// Thrown when a package cannot be found on a feed.
    /// </summary>
    public class PackageNotFoundProtocolException : InvalidCacheProtocolException
    {
        /// <summary>
        /// Package that was not found.
        /// </summary>
        public PackageIdentity PackageIdentity { get; }

        public PackageNotFoundProtocolException(PackageIdentity package)
            : base(GetMessage(package))
        {
            PackageIdentity = package ?? throw new ArgumentNullException(nameof(package));
        }

        public PackageNotFoundProtocolException(PackageIdentity package, Exception innerException)
            : base(GetMessage(package), innerException)
        {
            PackageIdentity = package ?? throw new ArgumentNullException(nameof(package));
        }

        private static string GetMessage(PackageIdentity package)
        {
            return string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PackageNotFound,
                    package.ToString());
        }
    }
}
