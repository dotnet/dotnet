// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.AspNetCore.Razor.Language;

public static class CommonMetadata
{
    internal static KeyValuePair<string, string?> MakeTrue(string key)
        => new(key, bool.TrueString);
    internal static KeyValuePair<string, string?> RuntimeName(string value)
        => new(TagHelperMetadata.Runtime.Name, value);
    internal static KeyValuePair<string, string?> SpecialKind(string value)
        => new(ComponentMetadata.SpecialKindKey, value);
    public static KeyValuePair<string, string?> TypeName(string value)
        => new(TagHelperMetadata.Common.TypeName, value);
    internal static KeyValuePair<string, string?> TypeNamespace(string value)
        => new(TagHelperMetadata.Common.TypeNamespace, value);
    internal static KeyValuePair<string, string?> TypeNameIdentifier(string value)
        => new(TagHelperMetadata.Common.TypeNameIdentifier, value);
}
