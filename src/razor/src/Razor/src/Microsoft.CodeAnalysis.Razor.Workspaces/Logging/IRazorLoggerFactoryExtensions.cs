// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal static class IRazorLoggerFactoryExtensions
{
    public static ILogger CreateLogger<T>(this IRazorLoggerFactory factory)
    {
        return factory.CreateLogger(TrimTypeName(typeof(T).FullName));
    }

    private static string TrimTypeName(string name)
    {
        string trimmedName;
        _ = TryTrim(name, "Microsoft.VisualStudio.", out trimmedName) ||
            TryTrim(name, "Microsoft.AspNetCore.Razor.", out trimmedName);

        return trimmedName;

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
