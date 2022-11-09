// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Extensions
{
    public enum ApiKind
    {
        Namespace = 0,
        Interface = 1,
        Delegate = 2,
        Enum,
        EnumField,
        Struct,
        Class,
        DelegateMember,
        Field,
        Property,
        Event,
        Constructor,
        PropertyAccessor,
        EventAccessor,
        Method
    }
}
