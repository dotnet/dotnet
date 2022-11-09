// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities.UnitTests;

[DebuggerStepThrough]
internal static class FindMethodExtensions
{
    private const BindingFlags PrivateBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    internal static MethodInfo? FindMethod(this Type type, string signature)
        => type.FindMembers(MemberTypes.Method, PrivateBindingFlags,
            (mbr, sig) => mbr.ToString() == (string?)sig, signature).FirstOrDefault() as MethodInfo;

    internal static IMethodSymbol FindMethod(
        this INamedTypeSymbol type,
        string methodName,
        int methodGenericArity = -1,
        params ITypeSymbol[] methodParameterTypes)
    {
        var candidates = GetCandidateMethods(type, methodName);
        if (candidates.Any() && !candidates.Skip(1).Any())
        {
            return candidates.Single();
        }

        if (methodGenericArity != -1)
        {
            candidates = candidates.Where(m => m.Arity == methodGenericArity);
            if (candidates.Any() && !candidates.Skip(1).Any())
            {
                return candidates.Single();
            }
        }

        if (methodParameterTypes != null && methodParameterTypes.Length >= 0)
        {
            candidates = candidates.Where(m => m.Parameters.Length == methodParameterTypes.Length);
            if (candidates.Any() && !candidates.Skip(1).Any())
            {
                return candidates.Single();
            }

            candidates = candidates.Where(m => m.Parameters.Select(p => p.Type).SequenceEqual(methodParameterTypes));
        }

        Debug.Assert(candidates.Any() && !candidates.Skip(1).Any());
        return candidates.Single();
    }

    internal static IMethodSymbol FindMethod(
        this INamedTypeSymbol type,
        string methodName,
        int methodGenericArity,
        int methodParameterCount,
        Func<IMethodSymbol, bool> selector)
    {
        var candidates = GetCandidateMethods(type, methodName);
        if (candidates.Any() && !candidates.Skip(1).Any())
        {
            return candidates.Single();
        }

        candidates = candidates.Where(m => m.Arity == methodGenericArity);
        if (candidates.Any() && !candidates.Skip(1).Any())
        {
            return candidates.Single();
        }

        candidates = candidates.Where(m => m.Parameters.Length == methodParameterCount);
        if (candidates.Any() && !candidates.Skip(1).Any())
        {
            return candidates.Single();
        }

        candidates = candidates.Where(selector);

        Debug.Assert(candidates.Any() && !candidates.Skip(1).Any());
        return candidates.Single();
    }

    private static IEnumerable<IMethodSymbol> GetCandidateMethods(INamedTypeSymbol type, string methodName)
    {
        var candidates = type.GetMembers(methodName).OfType<IMethodSymbol>();

        if (type.BaseType != null && type.BaseType.SpecialType != SpecialType.System_Object)
        {
            candidates = candidates.Union(GetCandidateMethods(type.BaseType, methodName));
        }

        return candidates;
    }
}
