// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    public class LicenseUrlDeprecationWarning : IPackageRule
    {
        public string MessageFormat { get; }

        public LicenseUrlDeprecationWarning(string messageFormat)
        {
            MessageFormat = messageFormat ?? throw new ArgumentNullException(nameof(messageFormat));
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {

            var nuspecReader = builder?.NuspecReader;
            var licenseMetadata = nuspecReader?.GetLicenseMetadata();
            var licenseUrl = nuspecReader?.GetLicenseUrl();

            if (licenseMetadata == null && licenseUrl != null)
            {
                yield return PackagingLogMessage.CreateWarning(
                    string.Format(CultureInfo.CurrentCulture, MessageFormat),
                    NuGetLogCode.NU5125);
            }
        }
    }
}
