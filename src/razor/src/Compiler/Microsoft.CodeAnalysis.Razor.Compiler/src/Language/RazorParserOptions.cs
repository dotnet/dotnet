// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class RazorParserOptions
{
    private static RazorLanguageVersion DefaultLanguageVersion => RazorLanguageVersion.Latest;
    private static string DefaultFileKind => FileKinds.Legacy;

    public static RazorParserOptions Default { get; } = new(
        languageVersion: DefaultLanguageVersion,
        fileKind: FileKinds.Legacy,
        directives: [],
        csharpParseOptions: CSharpParseOptions.Default,
        flags: GetDefaultFlags(DefaultLanguageVersion, DefaultFileKind));

    public RazorLanguageVersion LanguageVersion { get; }
    internal string FileKind { get; }

    public ImmutableArray<DirectiveDescriptor> Directives { get; }
    public CSharpParseOptions CSharpParseOptions { get; }

    private readonly Flags _flags;

    private RazorParserOptions(
        RazorLanguageVersion languageVersion,
        string fileKind,
        ImmutableArray<DirectiveDescriptor> directives,
        CSharpParseOptions csharpParseOptions,
        Flags flags)
    {
        if (flags.IsFlagSet(Flags.ParseLeadingDirectives) &&
            flags.IsFlagSet(Flags.UseRoslynTokenizer))
        {
            throw new ArgumentException($"Cannot set {nameof(Flags.ParseLeadingDirectives)} and {nameof(Flags.UseRoslynTokenizer)} to true simultaneously.");
        }

        LanguageVersion = languageVersion ?? DefaultLanguageVersion;
        FileKind = fileKind ?? DefaultFileKind;
        Directives = directives;
        CSharpParseOptions = csharpParseOptions;
        _flags = flags;
    }

    public bool DesignTime
        => _flags.IsFlagSet(Flags.DesignTime);

    /// <summary>
    /// Gets a value which indicates whether the parser will parse only the leading directives. If <c>true</c>
    /// the parser will halt at the first HTML content or C# code block. If <c>false</c> the whole document is parsed.
    /// </summary>
    /// <remarks>
    /// Currently setting this option to <c>true</c> will result in only the first line of directives being parsed.
    /// In a future release this may be updated to include all leading directive content.
    /// </remarks>
    public bool ParseLeadingDirectives
        => _flags.IsFlagSet(Flags.ParseLeadingDirectives);

    public bool UseRoslynTokenizer
        => _flags.IsFlagSet(Flags.UseRoslynTokenizer);

    internal bool EnableSpanEditHandlers
        => _flags.IsFlagSet(Flags.EnableSpanEditHandlers);

    internal bool AllowMinimizedBooleanTagHelperAttributes
        => _flags.IsFlagSet(Flags.AllowMinimizedBooleanTagHelperAttributes);

    internal bool AllowHtmlCommentsInTagHelpers
        => _flags.IsFlagSet(Flags.AllowHtmlCommentsInTagHelpers);

    internal bool AllowComponentFileKind
        => _flags.IsFlagSet(Flags.AllowComponentFileKind);

    internal bool AllowRazorInAllCodeBlocks
        => _flags.IsFlagSet(Flags.AllowRazorInAllCodeBlocks);

    internal bool AllowUsingVariableDeclarations
        => _flags.IsFlagSet(Flags.AllowUsingVariableDeclarations);

    internal bool AllowConditionalDataDashAttributes
        => _flags.IsFlagSet(Flags.AllowConditionalDataDashAttributes);

    internal bool AllowCSharpInMarkupAttributeArea
        => _flags.IsFlagSet(Flags.AllowCSharpInMarkupAttributeArea);

    internal bool AllowNullableForgivenessOperator
        => _flags.IsFlagSet(Flags.AllowNullableForgivenessOperator);

    public RazorParserOptions WithDirectives(params ImmutableArray<DirectiveDescriptor> value)
        => Directives.SequenceEqual(value)
            ? this
            : new(LanguageVersion, FileKind, value, CSharpParseOptions, _flags);

    public RazorParserOptions WithCSharpParseOptions(CSharpParseOptions value)
        => CSharpParseOptions.Equals(value)
            ? this
            : new(LanguageVersion, FileKind, Directives, value, _flags);

    public RazorParserOptions WithFlags(
        Optional<bool> designTime = default,
        Optional<bool> parseLeadingDirectives = default,
        Optional<bool> useRoslynTokenizer = default,
        Optional<bool> enableSpanEditHandlers = default,
        Optional<bool> allowMinimizedBooleanTagHelperAttributes = default,
        Optional<bool> allowHtmlCommentsInTagHelpers = default,
        Optional<bool> allowComponentFileKind = default,
        Optional<bool> allowRazorInAllCodeBlocks = default,
        Optional<bool> allowUsingVariableDeclarations = default,
        Optional<bool> allowConditionalDataDashAttributes = default,
        Optional<bool> allowCSharpInMarkupAttributeArea = default,
        Optional<bool> allowNullableForgivenessOperator = default)
    {
        var flags = _flags;

        if (designTime.HasValue)
        {
            flags.UpdateFlag(Flags.DesignTime, designTime.Value);
        }

        if (parseLeadingDirectives.HasValue)
        {
            flags.UpdateFlag(Flags.ParseLeadingDirectives, parseLeadingDirectives.Value);
        }

        if (useRoslynTokenizer.HasValue)
        {
            flags.UpdateFlag(Flags.UseRoslynTokenizer, useRoslynTokenizer.Value);
        }

        if (enableSpanEditHandlers.HasValue)
        {
            flags.UpdateFlag(Flags.EnableSpanEditHandlers, enableSpanEditHandlers.Value);
        }

        if (allowMinimizedBooleanTagHelperAttributes.HasValue)
        {
            flags.UpdateFlag(Flags.AllowMinimizedBooleanTagHelperAttributes, allowMinimizedBooleanTagHelperAttributes.Value);
        }

        if (allowHtmlCommentsInTagHelpers.HasValue)
        {
            flags.UpdateFlag(Flags.AllowHtmlCommentsInTagHelpers, allowHtmlCommentsInTagHelpers.Value);
        }

        if (allowComponentFileKind.HasValue)
        {
            flags.UpdateFlag(Flags.AllowComponentFileKind, allowComponentFileKind.Value);
        }

        if (allowRazorInAllCodeBlocks.HasValue)
        {
            flags.UpdateFlag(Flags.AllowRazorInAllCodeBlocks, allowRazorInAllCodeBlocks.Value);
        }

        if (allowUsingVariableDeclarations.HasValue)
        {
            flags.UpdateFlag(Flags.AllowUsingVariableDeclarations, allowUsingVariableDeclarations.Value);
        }

        if (allowConditionalDataDashAttributes.HasValue)
        {
            flags.UpdateFlag(Flags.AllowConditionalDataDashAttributes, allowConditionalDataDashAttributes.Value);
        }

        if (allowCSharpInMarkupAttributeArea.HasValue)
        {
            flags.UpdateFlag(Flags.AllowCSharpInMarkupAttributeArea, allowCSharpInMarkupAttributeArea.Value);
        }

        if (allowNullableForgivenessOperator.HasValue)
        {
            flags.UpdateFlag(Flags.AllowNullableForgivenessOperator, allowNullableForgivenessOperator.Value);
        }

        return flags == _flags
            ? this
            : new(LanguageVersion, FileKind, Directives, CSharpParseOptions, flags);
    }
}
