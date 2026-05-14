// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// Represents the expression type of a <see cref="NuGetLicenseExpression"/>.
    /// License type means that it's a <see cref="NuGetLicense"/>. Operator means that it's a <see cref="LicenseOperator"/>
    /// </summary>
    public enum LicenseExpressionType
    {
        License,
        Operator
    }
}
