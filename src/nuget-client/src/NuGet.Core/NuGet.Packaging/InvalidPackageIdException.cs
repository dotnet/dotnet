// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;

namespace NuGet.Packaging
{
    public class InvalidPackageIdException : ArgumentException
    {
        public InvalidPackageIdException(string message)
            : base(message)
        {
        }
    }
}
