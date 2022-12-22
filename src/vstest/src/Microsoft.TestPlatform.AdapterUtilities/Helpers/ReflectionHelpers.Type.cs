// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Microsoft.TestPlatform.AdapterUtilities.Helpers;

internal static partial class ReflectionHelpers
{
    internal static bool IsGenericType(Type type)
    {
        return type.IsGenericType;
    }

    internal static MethodBase? GetDeclaringMethod(Type type)
    {
        return type.DeclaringMethod;
    }
}
