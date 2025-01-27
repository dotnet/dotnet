// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal static class TagHelperDescriptorExtensions
{
    public static bool IsAnyComponentDocumentTagHelper(this TagHelperDescriptor tagHelper)
    {
        return tagHelper.IsComponentTagHelper || tagHelper.Metadata.ContainsKey(ComponentMetadata.SpecialKindKey);
    }

    public static bool IsBindTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            string.Equals(ComponentMetadata.Bind.TagHelperKind, kind);
    }

    public static bool IsFallbackBindTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.IsBindTagHelper() &&
            tagHelper.Metadata.TryGetValue(ComponentMetadata.Bind.FallbackKey, out var fallback) &&
            string.Equals(bool.TrueString, fallback);
    }

    public static bool IsFormNameTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            kind == ComponentMetadata.FormName.TagHelperKind;
    }

    public static bool IsGenericTypedComponent(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.IsComponentTagHelper &&
            tagHelper.Metadata.TryGetValue(ComponentMetadata.Component.GenericTypedKey, out var value) &&
            string.Equals(bool.TrueString, value);
    }

    /// <summary>
    /// Given a taghelper binding it finds the BoundAttribute that is a type parameter and then the
    /// actual binding value for that type.
    ///
    /// <code>
    /// &lt;MyTagHelper
    ///   TItem="string"
    ///   OnChange="OnMyTagHelperChange" /&gt;
    /// </code>
    ///
    /// The above code will return "string" for the typeName.
    /// </summary>
    /// <remarks>
    /// As of now this method only supports cases where there is a single bound attribute that is a type parameter. If there are multiple this returns false.
    /// </remarks>
#nullable enable
    public static bool TryGetGenericTypeNameFromComponent(this TagHelperDescriptor tagHelper, TagHelperBinding binding, [NotNullWhen(true)] out string? typeName)
    {
        typeName = null;

        if (!tagHelper.IsComponentTagHelper)
        {
            return false;
        }

        foreach (var boundAttribute in tagHelper.BoundAttributes)
        {
            // This is a bit of a headache so let me explain:
            // The bound attribute needs to be marked "True" for the "TypeParameter" key in order to be considered a type parameter.
            // The property name for that is the actual property we need to read, such as "TItem".
            // However, since you can't get the value from the TagHelperDescriptor directly (it's the type, not what the user has provided data to map)
            // it has to be looked up in the bindingAttributes to find the value for that type. This assumes that the type is valid because the user
            // provided it, and if it's not the calling context probably doesn't care.
            if (boundAttribute.IsTypeParameterProperty() &&
                boundAttribute.GetPropertyName() is string propertyName &&
                binding.Attributes.FirstOrDefault(propertyName, static (kvp, propertyName) => kvp.Key == propertyName) is { Value: var bindingTypeName })
            {
                if (typeName is not null)
                {
                    // Multiple generic types were found and currently not supported.
                    typeName = null;
                    return false;
                }

                typeName = bindingTypeName;
            }
        }

        return typeName is not null;
    }
#nullable disable

    public static bool IsInputElementBindTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.IsBindTagHelper() &&
            tagHelper.TagMatchingRules.Length == 2 &&
            string.Equals("input", tagHelper.TagMatchingRules[0].TagName);
    }

    public static bool IsInputElementFallbackBindTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.IsInputElementBindTagHelper() &&
            !tagHelper.Metadata.ContainsKey(ComponentMetadata.Bind.TypeAttribute);
    }

    public static string GetValueAttributeName(this TagHelperDescriptor tagHelper)
    {
        tagHelper.Metadata.TryGetValue(ComponentMetadata.Bind.ValueAttribute, out var result);
        return result;
    }

    public static string GetChangeAttributeName(this TagHelperDescriptor tagHelper)
    {
        tagHelper.Metadata.TryGetValue(ComponentMetadata.Bind.ChangeAttribute, out var result);
        return result;
    }

    public static string GetExpressionAttributeName(this TagHelperDescriptor tagHelper)
    {
        tagHelper.Metadata.TryGetValue(ComponentMetadata.Bind.ExpressionAttribute, out var result);
        return result;
    }

    /// <summary>
    /// Gets a value that indicates where the tag helper is a bind tag helper with a default
    /// culture value of <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    /// <param name="tagHelper">The <see cref="TagHelperDescriptor"/>.</param>
    /// <returns>
    /// <c>true</c> if this tag helper is a bind tag helper and defaults in <see cref="CultureInfo.InvariantCulture"/>
    /// </returns>
    public static bool IsInvariantCultureBindTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.Bind.IsInvariantCulture, out var text) &&
            bool.TryParse(text, out var result) &&
            result;
    }

    /// <summary>
    /// Gets the default format value for a bind tag helper.
    /// </summary>
    /// <param name="tagHelper">The <see cref="TagHelperDescriptor"/>.</param>
    /// <returns>The format, or <c>null</c>.</returns>
    public static string GetFormat(this TagHelperDescriptor tagHelper)
    {
        tagHelper.Metadata.TryGetValue(ComponentMetadata.Bind.Format, out var result);
        return result;
    }

    public static bool IsEventHandlerTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            string.Equals(ComponentMetadata.EventHandler.TagHelperKind, kind);
    }

    public static bool IsKeyTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            string.Equals(ComponentMetadata.Key.TagHelperKind, kind);
    }

    public static bool IsSplatTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            string.Equals(ComponentMetadata.Splat.TagHelperKind, kind);
    }

    public static bool IsRefTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            string.Equals(ComponentMetadata.Ref.TagHelperKind, kind);
    }

    public static bool IsRenderModeTagHelper(this TagHelperDescriptor tagHelper)
    {
        return
            tagHelper.Metadata.TryGetValue(ComponentMetadata.SpecialKindKey, out var kind) &&
            string.Equals(ComponentMetadata.RenderMode.TagHelperKind, kind);
    }

    public static string GetEventArgsType(this TagHelperDescriptor tagHelper)
    {
        tagHelper.Metadata.TryGetValue(ComponentMetadata.EventHandler.EventArgsType, out var result);
        return result;
    }

    /// <summary>
    /// Gets the set of component attributes that can accept child content (<c>RenderFragment</c> or <c>RenderFragment{T}</c>).
    /// </summary>
    /// <param name="tagHelper">The <see cref="TagHelperDescriptor"/>.</param>
    /// <returns>The child content attributes</returns>
    public static IEnumerable<BoundAttributeDescriptor> GetChildContentProperties(this TagHelperDescriptor tagHelper)
    {
        foreach (var attribute in tagHelper.BoundAttributes)
        {
            if (attribute.IsChildContentProperty())
            {
                yield return attribute;
            }
        }
    }

    /// <summary>
    /// Gets the set of component attributes that represent generic type parameters of the component type.
    /// </summary>
    /// <param name="tagHelper">The <see cref="TagHelperDescriptor"/>.</param>
    /// <returns>The type parameter attributes</returns>
    public static IEnumerable<BoundAttributeDescriptor> GetTypeParameters(this TagHelperDescriptor tagHelper)
    {
        foreach (var attribute in tagHelper.BoundAttributes)
        {
            if (attribute.IsTypeParameterProperty())
            {
                yield return attribute;
            }
        }
    }

    /// <summary>
    /// Gets a flag that indicates whether the corresponding component supplies any cascading
    /// generic type parameters to descendants.
    /// </summary>
    /// <param name="tagHelper">The <see cref="TagHelperDescriptor"/>.</param>
    /// <returns>True if it does supply one or more generic type parameters to descendants; false otherwise.</returns>
    public static bool SuppliesCascadingGenericParameters(this TagHelperDescriptor tagHelper)
    {
        foreach (var attribute in tagHelper.BoundAttributes)
        {
            if (attribute.IsCascadingTypeParameterProperty())
            {
                return true;
            }
        }

        return false;
    }
}
