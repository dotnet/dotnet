// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;

[Serializable]
public class InvalidManagedNameException : Exception, ISerializable
{
    public InvalidManagedNameException(string? message) : base(message) { }

    protected InvalidManagedNameException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
