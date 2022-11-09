// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Licenses;

namespace NuGet.Packaging.Rules
{
    internal class UnrecognizedLicenseIdentifierRule : IPackageRule
    {
        public string MessageFormat { get; }

        public UnrecognizedLicenseIdentifierRule(string messageFormat)
        {
            MessageFormat = messageFormat ?? throw new ArgumentNullException(nameof(messageFormat));
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            var nuspecReader = builder?.NuspecReader;
            var licenseMetadata = nuspecReader?.GetLicenseMetadata();
            if (licenseMetadata != null && licenseMetadata.Type == LicenseType.Expression && licenseMetadata.LicenseExpression != null)
            {
                var warnings = new List<PackagingLogMessage>();
                ValidateAllLicenseLeafs(licenseMetadata.LicenseExpression, warnings);
                return warnings;
            }
            return Enumerable.Empty<PackagingLogMessage>();
        }

        private void ValidateAllLicenseLeafs(NuGetLicenseExpression expression, IList<PackagingLogMessage> logMessages)
        {
            switch (expression.Type)
            {
                case LicenseExpressionType.License:
                    var license = (NuGetLicense)expression;
                    if (!license.IsStandardLicense)
                    {
                        logMessages.Add(
                            PackagingLogMessage.CreateWarning(
                                        string.Format(CultureInfo.CurrentCulture, MessageFormat, license.Identifier),
                                        NuGetLogCode.NU5124));
                    }
                    break;

                case LicenseExpressionType.Operator:
                    var licenseOperator = (LicenseOperator)expression;
                    switch (licenseOperator.OperatorType)
                    {
                        case LicenseOperatorType.LogicalOperator:
                            var logicalOperator = (LogicalOperator)licenseOperator;
                            ValidateAllLicenseLeafs(logicalOperator.Left, logMessages);
                            ValidateAllLicenseLeafs(logicalOperator.Right, logMessages);
                            break;

                        case LicenseOperatorType.WithOperator:
                            var withOperator = (WithOperator)licenseOperator;
                            ValidateAllLicenseLeafs(withOperator.License, logMessages);
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
