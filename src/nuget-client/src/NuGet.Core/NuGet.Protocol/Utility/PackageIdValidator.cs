// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Packaging;

namespace NuGet.Protocol
{
    internal static class PackageIdValidator
    {
        /// <summary>
        /// Validates the package ID content.
        /// </summary>
        /// <param name="packageId">The package ID to validate.</param>
        /// <exception cref="InvalidPackageIdException">
        /// Thrown if <paramref name="packageId"/> is not a valid NuGet package ID.
        /// </exception>
        internal static void Validate(string packageId, IEnvironmentVariableReader env = null)
        {
            if (env == null)
            {
                env = EnvironmentVariableWrapper.Instance;
            }

            string disableValidationEnvVarValue = env.GetEnvironmentVariable("NUGET_DISABLE_PACKAGEID_VALIDATION");

            if (!string.Equals(disableValidationEnvVarValue, "true", StringComparison.OrdinalIgnoreCase))
            {
                if (!Packaging.PackageIdValidator.IsValidPackageId(packageId))
                {
                    throw new InvalidPackageIdException(string.Format(CultureInfo.CurrentCulture, Strings.Error_Invalid_package_id, packageId));
                }
            }
        }
    }
}
