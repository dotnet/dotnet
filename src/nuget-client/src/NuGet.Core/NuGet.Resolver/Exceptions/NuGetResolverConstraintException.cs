// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGet.Resolver
{
    /// <summary>
    /// Constraint exception. Thrown when a solution cannot be found.
    /// </summary>
    [Serializable]
    public class NuGetResolverConstraintException : NuGetResolverException
    {
        public NuGetResolverConstraintException(string message)
            : base(message)
        {
        }

        protected NuGetResolverConstraintException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
