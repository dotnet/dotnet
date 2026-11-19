// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGetClone.Frameworks
{
    [Serializable]
    internal class FrameworkException : Exception // CodeQL [SM02227] Not deserialized with an unsafe formatter; the serialization constructor only forwards to the Exception base constructor.
    {
        public FrameworkException(string message)
            : base(message)
        {
        }

        protected FrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
