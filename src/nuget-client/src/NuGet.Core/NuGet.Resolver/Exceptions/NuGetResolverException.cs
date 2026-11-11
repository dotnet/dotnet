// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Resolver
{
    public class NuGetResolverException : Exception
    {
        public NuGetResolverException(string message)
            : base(message)
        {
        }
    }
}
