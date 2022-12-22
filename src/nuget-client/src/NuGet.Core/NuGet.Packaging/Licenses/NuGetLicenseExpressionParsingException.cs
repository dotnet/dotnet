// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Runtime.Serialization;

namespace NuGet.Packaging.Licenses
{
    [Serializable]
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

        protected NuGetLicenseExpressionParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
