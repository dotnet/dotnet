// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Microsoft.TestPlatform.AdapterUtilities.Helpers;

internal static partial class ReflectionHelpers
{
    internal static bool IsMethod(MethodBase method)
    {
        return method.MemberType == MemberTypes.Method;
    }

    internal static Type? GetReflectedType(MethodBase method)
    {
        return method.ReflectedType;
    }

    internal static RuntimeMethodHandle GetMethodHandle(MethodBase method)
    {
        return method.MethodHandle;
    }
}
