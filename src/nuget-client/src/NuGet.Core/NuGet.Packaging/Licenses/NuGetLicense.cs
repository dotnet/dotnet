// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// NuGet's internal representation of a license identifier. 
    /// </summary>
    public class NuGetLicense : NuGetLicenseExpression
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Signifies whether the plus operator has been specified on this license
        /// </summary>
        public bool Plus { get; }

        /// <summary>
        /// Signifies whether this is a standard license known by the NuGet APIs.
        /// Pack for example should warn for these.
        /// </summary>
        public bool IsStandardLicense { get; }

        private NuGetLicense(string identifier, bool plus, bool isStandardLicense)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            Plus = plus;
            IsStandardLicense = isStandardLicense;
            Type = LicenseExpressionType.License;
        }

        /// <summary>
        /// Parse a licenseIdentifier. If a licenseIdentifier is deprecated, this will throw. Non-standard licenses get parsed into a object model as well.
        /// </summary>
        /// <param name="licenseIdentifier">license identifier to be parsed</param>
        /// <param name="allowUnlicensed">Whether the parser allows the UNLICENSED identifier</param>
        /// <returns>Prased NuGetLicense object</returns>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the identifier is deprecated, contains invalid characters or is an exception identifier.</exception>
        /// <exception cref="ArgumentException">If it's null or empty.</exception>
        /// <remarks>The purpose of the <paramref name="allowUnlicensed"/> switch is to allow the expression parser communicate at which operand location the unlicensed identifier is legal. </remarks>
        internal static NuGetLicense ParseIdentifier(string licenseIdentifier, bool allowUnlicensed = false)
        {
            if (!string.IsNullOrWhiteSpace(licenseIdentifier))
            {
                if (NuGetLicenseData.LicenseList.TryGetValue(licenseIdentifier, out var licenseData))
                {
                    return !licenseData.IsDeprecatedLicenseId ?
                        new NuGetLicense(licenseIdentifier, plus: false, isStandardLicense: true) :
                        throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_DeprecatedIdentifier, licenseIdentifier));
                }
                else
                {
                    if (licenseIdentifier[licenseIdentifier.Length - 1] == '+')
                    {
                        var cleanIdentifier = licenseIdentifier.Substring(0, licenseIdentifier.Length - 1);
                        var plus = true;
                        if (NuGetLicenseData.LicenseList.TryGetValue(cleanIdentifier, out licenseData))
                        {
                            return !licenseData.IsDeprecatedLicenseId ?
                                new NuGetLicense(cleanIdentifier, plus: plus, isStandardLicense: true) :
                                throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_DeprecatedIdentifier, licenseIdentifier));
                        }
                        return ProcessLicenseNotInStandardData(cleanIdentifier, plus: plus, allowUnlicensed: allowUnlicensed);
                    }

                    return ProcessLicenseNotInStandardData(licenseIdentifier, plus: false, allowUnlicensed: allowUnlicensed);
                }
            }
            // This will not happen in production code as the tokenizer takes cares of that. 
            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeNullOrEmpty, nameof(licenseIdentifier)));
        }

        /// <summary>
        /// The valid characters for a license identifier are a-zA-Z0-9.-
        /// This method assumes that the trailing + operator has been stripped out.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <returns>whether the value has valid characters</returns>
        private static bool HasValidCharacters(string value)
        {
            var regex = new Regex("^[a-zA-Z0-9\\.\\-]+$", RegexOptions.CultureInvariant);
            return regex.IsMatch(value);
        }

        private static NuGetLicense ProcessLicenseNotInStandardData(string licenseIdentifier, bool plus, bool allowUnlicensed)
        {
            if (!NuGetLicenseData.ExceptionList.TryGetValue(licenseIdentifier, out var exceptionData))
            {
                if (HasValidCharacters(licenseIdentifier))
                {
                    if (licenseIdentifier.Equals(UNLICENSED, StringComparison.Ordinal))
                    {
                        if (plus)
                        {
                            throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_IllegalUnlicensedOperator));
                        }
                        if (allowUnlicensed)
                        {
                            return new NuGetLicense(licenseIdentifier, plus: false, isStandardLicense: true); // UNLICENSED is considered a standard license.
                        }
                        throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_UnexpectedIdentifier, licenseIdentifier));
                    }
                    else
                    {
                        return new NuGetLicense(licenseIdentifier, plus: plus, isStandardLicense: false);
                    }
                }
                else
                {
                    throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_LicenseInvalidCharacters, licenseIdentifier));
                }
            }
            throw new NuGetLicenseExpressionParsingException(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_LicenseIdentifierIsException, licenseIdentifier));
        }

        internal static string UNLICENSED = "UNLICENSED";

        public override string ToString()
        {
            var plus = Plus ? "+" : string.Empty;
            return $"{Identifier}{plus}";
        }
    }
}
