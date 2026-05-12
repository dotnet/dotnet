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
        private const string DisableValidationEnvVar = "NUGET_DISABLE_PACKAGEID_VALIDATION";

        private static readonly Lazy<bool> IsValidationDisabled = new Lazy<bool>(() =>
            IsPackageIdValidationDisabled(EnvironmentVariableWrapper.Instance));

        /// <summary>
        /// Validates the package ID content.
        /// </summary>
        /// <param name="packageId">The package ID to validate.</param>
        /// <exception cref="InvalidPackageIdException">
        /// Thrown if <paramref name="packageId"/> is not a valid NuGet package ID.
        /// </exception>
        internal static void Validate(string packageId, IEnvironmentVariableReader env = null)
        {
            bool isDisabled = env == null
                ? IsValidationDisabled.Value
                : IsPackageIdValidationDisabled(env);

            if (!isDisabled)
            {
                if (!Packaging.PackageIdValidator.IsValidPackageId(packageId))
                {
                    throw new InvalidPackageIdException(string.Format(CultureInfo.CurrentCulture, Strings.Error_Invalid_package_id, packageId));
                }
            }
        }

        private static bool IsPackageIdValidationDisabled(IEnvironmentVariableReader env) =>
            string.Equals(env.GetEnvironmentVariable(DisableValidationEnvVar), bool.TrueString, StringComparison.OrdinalIgnoreCase);
    }
}
