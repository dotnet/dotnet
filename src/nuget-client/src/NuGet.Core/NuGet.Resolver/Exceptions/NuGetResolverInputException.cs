// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Resolver
{
    /// <summary>
    /// Input validation exception
    /// </summary>
    public class NuGetResolverInputException : NuGetResolverException
    {
        public NuGetResolverInputException(string message)
            : base(message)
        {
        }
    }
}
