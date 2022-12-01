// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

// Copied from https://github/dotnet/runtime

#if !NETCOREAPP3_0_OR_GREATER

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
    public CallerArgumentExpressionAttribute(string parameterName)
    {
        ParameterName = parameterName;
    }

    public string ParameterName { get; }
}

#endif
