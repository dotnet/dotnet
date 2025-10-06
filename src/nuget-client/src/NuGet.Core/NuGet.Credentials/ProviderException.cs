// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#if IS_DESKTOP
using System.Runtime.Serialization;
#endif

namespace NuGet.Credentials
{
    [Serializable]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ProviderException : Exception
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ProviderException()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ProviderException(string message) : base(message)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ProviderException(string message, Exception inner) : base(message, inner)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
        }
#if IS_DESKTOP
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected ProviderException(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}
