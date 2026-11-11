// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Resolver
{
    /// <summary>
    /// Constraint exception. Thrown when a solution cannot be found.
    /// </summary>
    public class NuGetResolverConstraintException : NuGetResolverException
    {
        public NuGetResolverConstraintException(string message)
            : base(message)
        {
        }
    }
}
