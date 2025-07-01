// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static class Extensions
{
    public static bool TryGetRazorParser(this PropertyCollection properties, [NotNullWhen(true)] out IRazorParser? parser)
    {
        if (properties.TryGetProperty(typeof(IVisualStudioRazorParser), out IVisualStudioRazorParser obj))
        {
            parser = RazorWrapperFactory.WrapParser(obj);
            return true;
        }

        parser = null;
        return false;
    }
}
