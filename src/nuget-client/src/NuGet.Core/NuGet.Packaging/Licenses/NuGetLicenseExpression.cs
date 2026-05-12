// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// Represents a parsed NuGetLicenseExpression.
    /// This is an abstract class so based on the Type, it can be either a <see cref="NuGetLicense"/> or a <see cref="LicenseOperator"/>.
    /// <seealso cref="LicenseExpressionType"/>
    /// </summary>
    public abstract class NuGetLicenseExpression
    {
        /// <summary>
        /// The type of the NuGetLicenseExpression.
        /// License type means that it's a <see cref="NuGetLicense"/>.
        /// Operator means that it's a <see cref="LicenseOperator"/>
        /// </summary>
        public LicenseExpressionType Type { get; protected set; }

        /// <summary>
        /// Parses a License Expression if valid.
        /// The expression would be parsed correct, even if non-standard exceptions are encountered.
        /// The non-standard Licenses/Exceptions have metadata on them with which the caller can make decisions.
        /// </summary>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>Parsed NuGet License Expression model.</returns>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the expression is empty or null.</exception>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the expression has invalid characters</exception>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the expression itself is invalid. Example: MIT OR OR Apache-2.0, or the MIT or Apache-2.0, because the expressions are case sensitive.</exception>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the expression's brackets are mismatched.</exception>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the licenseIdentifier is deprecated.</exception>
        /// <exception cref="NuGetLicenseExpressionParsingException">If the exception identifier is deprecated.</exception>
        public static NuGetLicenseExpression Parse(string expression)
        {
            return NuGetLicenseExpressionParser.Parse(expression);
        }

    }
}
