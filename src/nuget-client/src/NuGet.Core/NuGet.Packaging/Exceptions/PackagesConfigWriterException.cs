// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public class PackagesConfigWriterException : PackagingException
    {
        public PackagesConfigWriterException(string message)
            : base(message)
        {
        }

        public PackagesConfigWriterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
