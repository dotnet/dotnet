// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// A NuGetLicenseOperator. The operator options are: WITH or Logical operator, AND and OR.
    /// This is an abstract class so based on the NuGetLicenseOperatorType, it can be either a <see cref="WithOperator"/> or a <see cref="LogicalOperator"/>.
    /// </summary>
    public abstract class LicenseOperator : NuGetLicenseExpression
    {
        /// <summary>
        /// The operator type.
        /// LogicalOperator means it's AND or OR and <see cref="LogicalOperator"/>
        /// NuGetLicenseWithOperator means it's the WITH operator and <see cref="WithOperator"/>
        /// </summary>
        public LicenseOperatorType OperatorType { get; }

        protected LicenseOperator(LicenseOperatorType operatorType)
        {
            Type = LicenseExpressionType.Operator;
            OperatorType = operatorType;
        }
    }
}
