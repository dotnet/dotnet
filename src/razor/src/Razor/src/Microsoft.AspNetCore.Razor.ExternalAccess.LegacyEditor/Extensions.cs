// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static class Extensions
{
    public static bool TryGetRazorParser(this PropertyCollection properties, [NotNullWhen(true)] out IRazorParser? parser)
    {
        if (properties.TryGetProperty(typeof(VisualStudioRazorParser), out VisualStudioRazorParser obj))
        {
            parser = RazorWrapperFactory.WrapParser(obj);
            return true;
        }

        parser = null;
        return false;
    }
}
