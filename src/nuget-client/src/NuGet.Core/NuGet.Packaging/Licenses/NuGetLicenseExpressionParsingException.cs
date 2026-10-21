// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Licenses
{
    public class NuGetLicenseExpressionParsingException : Exception
    {
        public NuGetLicenseExpressionParsingException(string message)
            : base(message)
        {
        }

        public NuGetLicenseExpressionParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
