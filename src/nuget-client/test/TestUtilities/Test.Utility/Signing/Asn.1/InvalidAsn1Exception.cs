// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;

namespace Test.Utility.Signing
{
    [Serializable]
    public sealed class InvalidAsn1Exception : AsnContentException
    {
        public InvalidAsn1Exception() { }
        public InvalidAsn1Exception(string message) : base(message) { }
        public InvalidAsn1Exception(string message, Exception inner) : base(message, inner) { }
    }
}
