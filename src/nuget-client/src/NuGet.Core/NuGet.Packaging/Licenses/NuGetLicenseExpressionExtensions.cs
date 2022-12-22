// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Licenses
{
    public static class NuGetLicenseExpressionExtensions
    {
        /// <summary>
        /// Determines whether all the licenses and exceptions are not deprecated.
        /// </summary>
        /// <param name="expression">expression to be validated</param>
        /// <returns>Whether this expression consists of licenses with standard identifiers</returns>
        public static bool HasOnlyStandardIdentifiers(this NuGetLicenseExpression expression)
        {
            switch (expression.Type)
            {
                case LicenseExpressionType.License:
                    return (expression as NuGetLicense).IsStandardLicense;

                case LicenseExpressionType.Operator:
                    var licenseOperator = expression as LicenseOperator;
                    switch (licenseOperator.OperatorType)
                    {
                        case LicenseOperatorType.LogicalOperator:
                            var logicalOperator = (LogicalOperator)licenseOperator;
                            return logicalOperator.Left.HasOnlyStandardIdentifiers() && logicalOperator.Right.HasOnlyStandardIdentifiers();
                        case LicenseOperatorType.WithOperator:
                            var withOperator = (WithOperator)licenseOperator;
                            return withOperator.License.IsStandardLicense;
                        default:
                            return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// A leaf node in an expression can only be a License or an Exception. Run a func on each one.
        /// </summary>
        /// <param name="expression">The expression to be walked.</param>
        /// <param name="licenseProcessor">A processor for the licenses.</param>
        /// <param name="exceptionProcessor">A processor for the exceptions.</param>
        public static void OnEachLeafNode(this NuGetLicenseExpression expression, Action<NuGetLicense> licenseProcessor, Action<NuGetLicenseException> exceptionProcessor)
        {
            switch (expression.Type)
            {
                case LicenseExpressionType.License:
                    var license = (NuGetLicense)expression;
                    licenseProcessor?.Invoke(license);
                    break;

                case LicenseExpressionType.Operator:
                    var licenseOperator = (LicenseOperator)expression;
                    switch (licenseOperator.OperatorType)
                    {
                        case LicenseOperatorType.LogicalOperator:
                            var logicalOperator = (LogicalOperator)licenseOperator;

                            logicalOperator.Left.OnEachLeafNode(licenseProcessor, exceptionProcessor);
                            logicalOperator.Right.OnEachLeafNode(licenseProcessor, exceptionProcessor);
                            break;

                        case LicenseOperatorType.WithOperator:
                            var withOperator = (WithOperator)licenseOperator;
                            licenseProcessor?.Invoke(withOperator.License);
                            exceptionProcessor?.Invoke(withOperator.Exception);
                            break;

                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public static bool IsUnlicensed(this NuGetLicense license)
        {
            return license.Identifier.Equals(NuGetLicense.UNLICENSED, StringComparison.Ordinal);
        }

        public static bool IsUnlicensed(this NuGetLicenseExpression expression)
        {
            switch (expression.Type)
            {
                case LicenseExpressionType.License:
                    return ((NuGetLicense)expression).IsUnlicensed();

                case LicenseExpressionType.Operator: // expressions with operators cannot be unlicensed.
                default:
                    return false;
            }
        }
    }
}
