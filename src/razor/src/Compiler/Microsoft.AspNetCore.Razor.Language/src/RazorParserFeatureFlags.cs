// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class RazorParserFeatureFlags(
    bool allowMinimizedBooleanTagHelperAttributes,
    bool allowHtmlCommentsInTagHelpers,
    bool allowComponentFileKind,
    bool allowRazorInAllCodeBlocks,
    bool allowUsingVariableDeclarations,
    bool allowConditionalDataDashAttributesInComponents,
    bool allowCSharpInMarkupAttributeArea,
    bool allowNullableForgivenessOperator)
{
    public static RazorParserFeatureFlags Create(RazorLanguageVersion version, string fileKind)
    {
        if (fileKind == null)
        {
            throw new ArgumentNullException(nameof(fileKind));
        }

        var allowMinimizedBooleanTagHelperAttributes = false;
        var allowHtmlCommentsInTagHelpers = false;
        var allowComponentFileKind = false;
        var allowRazorInAllCodeBlocks = false;
        var allowUsingVariableDeclarations = false;
        var allowConditionalDataDashAttributes = false;
        var allowCSharpInMarkupAttributeArea = true;
        var allowNullableForgivenessOperator = false;

        if (version.CompareTo(RazorLanguageVersion.Version_2_1) >= 0)
        {
            // Added in 2.1
            allowMinimizedBooleanTagHelperAttributes = true;
            allowHtmlCommentsInTagHelpers = true;
        }

        if (version.CompareTo(RazorLanguageVersion.Version_3_0) >= 0)
        {
            // Added in 3.0
            allowComponentFileKind = true;
            allowRazorInAllCodeBlocks = true;
            allowUsingVariableDeclarations = true;
            allowNullableForgivenessOperator = true;
        }

        if (FileKinds.IsComponent(fileKind))
        {
            allowConditionalDataDashAttributes = true;
            allowCSharpInMarkupAttributeArea = false;
        }

        if (version.CompareTo(RazorLanguageVersion.Experimental) >= 0)
        {
            allowConditionalDataDashAttributes = true;
        }

        return new RazorParserFeatureFlags(
            allowMinimizedBooleanTagHelperAttributes,
            allowHtmlCommentsInTagHelpers,
            allowComponentFileKind,
            allowRazorInAllCodeBlocks,
            allowUsingVariableDeclarations,
            allowConditionalDataDashAttributes,
            allowCSharpInMarkupAttributeArea,
            allowNullableForgivenessOperator);
    }

    public bool AllowMinimizedBooleanTagHelperAttributes { get; } = allowMinimizedBooleanTagHelperAttributes;

    public bool AllowHtmlCommentsInTagHelpers { get; } = allowHtmlCommentsInTagHelpers;

    public bool AllowComponentFileKind { get; } = allowComponentFileKind;

    public bool AllowRazorInAllCodeBlocks { get; } = allowRazorInAllCodeBlocks;

    public bool AllowUsingVariableDeclarations { get; } = allowUsingVariableDeclarations;

    public bool AllowConditionalDataDashAttributes { get; } = allowConditionalDataDashAttributesInComponents;

    public bool AllowCSharpInMarkupAttributeArea { get; } = allowCSharpInMarkupAttributeArea;

    public bool AllowNullableForgivenessOperator { get; } = allowNullableForgivenessOperator;
}
