// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public static class TagHelperDescriptorExtensions
{
    /// <summary>
    /// Indicates whether a <see cref="TagHelperDescriptor"/> represents a view component.
    /// </summary>
    /// <param name="tagHelper">The <see cref="TagHelperDescriptor"/> to check.</param>
    /// <returns>Whether a <see cref="TagHelperDescriptor"/> represents a view component.</returns>
    public static bool IsViewComponentKind(this TagHelperDescriptor tagHelper)
    {
        ArgHelper.ThrowIfNull(tagHelper);

        return tagHelper.Kind == TagHelperKind.ViewComponent;
    }

    public static string? GetViewComponentName(this TagHelperDescriptor tagHelper)
    {
        ArgHelper.ThrowIfNull(tagHelper);

        return tagHelper.Metadata is ViewComponentMetadata { Name: var result }
            ? result
            : null;
    }
}
