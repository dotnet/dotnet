// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGet.Resolver
{
    /// <summary>
    /// Input validation exception
    /// </summary>
    [Serializable]
    public class NuGetResolverInputException : NuGetResolverException
    {
        public NuGetResolverInputException(string message)
            : base(message)
        {
        }

        protected NuGetResolverInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
