// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// NuGet's internal representation of a license exception identifier. 
    /// </summary>
    public class NuGetLicenseException
    {
        /// <summary>
        /// The Exception's identifier
        /// </summary>
        public string Identifier { get; }

        private NuGetLicenseException(string identifier)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// Parse an exceptionIdentifier. If the exceptionIdentifier is deprecated, this will throw. Non-standard exception do not get parsed into an object model.
        /// </summary>
        /// <param name="exceptionIdentifier">Exception identifier to be parsed.</param>
        /// <returns>Parsed License Exception</returns>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the identifier is deprecated, is a license or simply does not exist.</exception>
        /// <exception cref="ArgumentException">If it's null or empty.</exception>
        internal static NuGetLicenseException ParseIdentifier(string exceptionIdentifier)
        {
            if (!string.IsNullOrWhiteSpace(exceptionIdentifier))
            {
                if (NuGetLicenseData.ExceptionList.TryGetValue(exceptionIdentifier, out var exceptionData))
                {
                    return !exceptionData.IsDeprecatedLicenseId ?
                        new NuGetLicenseException(exceptionIdentifier) :
                        throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_DeprecatedIdentifier, exceptionIdentifier));
                }
                else
                {
                    if (NuGetLicenseData.LicenseList.TryGetValue(exceptionIdentifier, out var licenseData))
                    {
                        throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_ExceptionIdentifierIsLicense, exceptionIdentifier));
                    }
                    else
                    {
                        throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_InvalidExceptionIdentifier, exceptionIdentifier));
                    }
                }
            }
            // This will not happen in production code as the tokenizer takes cares of that. 
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeNullOrEmpty, nameof(exceptionIdentifier)));
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
