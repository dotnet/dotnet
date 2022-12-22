// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class IEnumerableExtensions
{
    internal static IEnumerable<T> WithoutNull<T>(this IEnumerable<T?> ts)
    {
        return ts.Where(t => t != null).Select(t => t!);
    }
}
