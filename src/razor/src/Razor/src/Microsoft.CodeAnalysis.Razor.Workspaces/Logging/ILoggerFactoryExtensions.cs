// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal static class ILoggerFactoryExtensions
{
    public static ILogger GetOrCreateLogger<T>(this ILoggerFactory factory)
    {
        return factory.GetOrCreateLogger(typeof(T));
    }

    public static ILogger GetOrCreateLogger(this ILoggerFactory factory, Type type)
    {
        return factory.GetOrCreateLogger(TrimTypeName(type.FullName.AssumeNotNull()));
    }

    private static string TrimTypeName(string name)
    {
        if (TryTrim(name, "Microsoft.VisualStudio.", out var trimmedName) ||
            TryTrim(name, "Microsoft.AspNetCore.Razor.", out trimmedName) ||
            TryTrim(name, "Microsoft.CodeAnalysis.Razor.", out trimmedName))
        {
            return trimmedName;
        }

        return name;

        static bool TryTrim(string name, string prefix, out string trimmedName)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                trimmedName = name.Substring(prefix.Length);
                return true;
            }

            trimmedName = name;

            return false;
        }
    }
}
