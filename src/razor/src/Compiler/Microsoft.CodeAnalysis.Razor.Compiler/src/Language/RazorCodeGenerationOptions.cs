// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class RazorCodeGenerationOptions
{
    private static RazorLanguageVersion DefaultLanguageVersion => RazorLanguageVersion.Latest;
    private static string DefaultFileKind => FileKinds.Legacy;
    private static int DefaultIndentSize => 4;
    private static string DefaultNewLine => Environment.NewLine;

    public static RazorCodeGenerationOptions Default { get; } = new(
        languageVersion: DefaultLanguageVersion,
        fileKind: DefaultFileKind,
        indentSize: DefaultIndentSize,
        newLine: DefaultNewLine,
        rootNamespace: null,
        suppressUniqueIds: null,
        flags: Flags.DefaultFlags);

    public static RazorCodeGenerationOptions DesignTimeDefault { get; } = new(
        languageVersion: DefaultLanguageVersion,
        fileKind: DefaultFileKind,
        indentSize: DefaultIndentSize,
        newLine: DefaultNewLine,
        rootNamespace: null,
        suppressUniqueIds: null,
        flags: Flags.DefaultDesignTimeFlags);

    public RazorLanguageVersion LanguageVersion { get; }
    internal string FileKind { get; }

    public int IndentSize { get; }
    public string NewLine { get; }

    /// <summary>
    /// Gets the root namespace for the generated code.
    /// </summary>
    public string? RootNamespace { get; }

    /// <summary>
    /// Gets a value used for unique ids for testing purposes. Null for unique ids.
    /// </summary>
    public string? SuppressUniqueIds { get; }

    private readonly Flags _flags;

    private RazorCodeGenerationOptions(
        RazorLanguageVersion languageVersion,
        string fileKind,
        int indentSize,
        string newLine,
        string? rootNamespace,
        string? suppressUniqueIds,
        Flags flags)
    {
        LanguageVersion = languageVersion ?? DefaultLanguageVersion;
        FileKind = fileKind ?? DefaultFileKind;
        IndentSize = indentSize;
        NewLine = newLine;
        RootNamespace = rootNamespace;
        SuppressUniqueIds = suppressUniqueIds;
        _flags = flags;
    }

    public bool DesignTime
        => _flags.HasFlag(Flags.DesignTime);

    public bool IndentWithTabs
        => _flags.HasFlag(Flags.IndentWithTabs);

    /// <summary>
    /// Gets a value that indicates whether to suppress the default <c>#pragma checksum</c> directive in the
    /// generated C# code. If <c>false</c> the checksum directive will be included, otherwise it will not be
    /// generated. Defaults to <c>false</c>, meaning that the checksum will be included.
    /// </summary>
    /// <remarks>
    /// The <c>#pragma checksum</c> is required to enable debugging and should only be suppressed for testing
    /// purposes.
    /// </remarks>
    public bool SuppressChecksum
        => _flags.HasFlag(Flags.SuppressChecksum);

    /// <summary>
    /// Gets a value that indicates whether to suppress the default metadata attributes in the generated
    /// C# code. If <c>false</c> the default attributes will be included, otherwise they will not be generated.
    /// Defaults to <c>false</c> at run time, meaning that the attributes will be included. Defaults to
    /// <c>true</c> at design time, meaning that the attributes will not be included.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>Microsoft.AspNetCore.Razor.Runtime</c> package includes a default set of attributes intended
    /// for runtimes to discover metadata about the compiled code.
    /// </para>
    /// <para>
    /// The default metadata attributes should be suppressed if code generation targets a runtime without
    /// a reference to <c>Microsoft.AspNetCore.Razor.Runtime</c>, or for testing purposes.
    /// </para>
    /// </remarks>
    public bool SuppressMetadataAttributes
        => _flags.HasFlag(Flags.SuppressMetadataAttributes);

    /// <summary>
    /// Gets a value that indicates whether to suppress the <c>RazorSourceChecksumAttribute</c>.
    /// <para>
    /// Used by default in .NET 6 apps since including a type-level attribute that changes on every
    /// edit are treated as rude edits by hot reload.
    /// </para>
    /// </summary>
    public bool SuppressMetadataSourceChecksumAttributes
        => _flags.HasFlag(Flags.SuppressMetadataSourceChecksumAttributes);

    /// <summary>
    /// Gets or sets a value that determines if an empty body is generated for the primary method.
    /// </summary>
    public bool SuppressPrimaryMethodBody
        => _flags.HasFlag(Flags.SuppressPrimaryMethodBody);

    /// <summary>
    /// Gets a value that determines if nullability type enforcement should be suppressed for user code.
    /// </summary>
    public bool SuppressNullabilityEnforcement
        => _flags.HasFlag(Flags.SuppressNullabilityEnforcement);

    /// <summary>
    /// Gets a value that determines if the components code writer may omit values for minimized attributes.
    /// </summary>
    public bool OmitMinimizedComponentAttributeValues
        => _flags.HasFlag(Flags.OmitMinimizedComponentAttributeValues);

    /// <summary>
    /// Gets a value that determines if localized component names are to be supported.
    /// </summary>
    public bool SupportLocalizedComponentNames
        => _flags.HasFlag(Flags.SupportLocalizedComponentNames);

    /// <summary>
    /// Gets a value that determines if enhanced line pragmas are to be utilized.
    /// </summary>
    public bool UseEnhancedLinePragma
        => _flags.HasFlag(Flags.UseEnhancedLinePragma);

    /// <summary>
    /// Determines whether RenderTreeBuilder.AddComponentParameter should not be used.
    /// </summary>
    public bool SuppressAddComponentParameter
        => _flags.HasFlag(Flags.SuppressAddComponentParameter);

    /// <summary>
    /// Determines if the file paths emitted as part of line pragmas should be mapped back to a valid path on windows.
    /// </summary>
    public bool RemapLinePragmaPathsOnWindows
        => _flags.HasFlag(Flags.RemapLinePragmaPathsOnWindows);

    public RazorCodeGenerationOptions WithIndentSize(int value)
        => IndentSize == value
            ? this
            : new(LanguageVersion, FileKind, value, NewLine, RootNamespace, SuppressUniqueIds, _flags);

    public RazorCodeGenerationOptions WithNewLine(string value)
        => NewLine == value
            ? this
            : new(LanguageVersion, FileKind, IndentSize, value, RootNamespace, SuppressUniqueIds, _flags);

    public RazorCodeGenerationOptions WithRootNamespace(string? value)
        => RootNamespace == value
            ? this
            : new(LanguageVersion, FileKind, IndentSize, NewLine, value, SuppressUniqueIds, _flags);

    public RazorCodeGenerationOptions WithSuppressUniqueIds(string? value)
        => RootNamespace == value
            ? this
            : new(LanguageVersion, FileKind, IndentSize, NewLine, RootNamespace, value, _flags);

    public RazorCodeGenerationOptions WithFlags(
        Optional<bool> designTime = default,
        Optional<bool> indentWithTabs = default,
        Optional<bool> suppressChecksum = default,
        Optional<bool> suppressMetadataAttributes = default,
        Optional<bool> suppressMetadataSourceChecksumAttributes = default,
        Optional<bool> suppressPrimaryMethodBody = default,
        Optional<bool> suppressNullabilityEnforcement = default,
        Optional<bool> omitMinimizedComponentAttributeValues = default,
        Optional<bool> supportLocalizedComponentNames = default,
        Optional<bool> useEnhancedLinePragma = default,
        Optional<bool> suppressAddComponentParameter = default,
        Optional<bool> remapLinePragmaPathsOnWindows = default)
    {
        var flags = _flags;

        if (designTime.HasValue)
        {
            flags.UpdateFlag(Flags.DesignTime, designTime.Value);
        }

        if (indentWithTabs.HasValue)
        {
            flags.UpdateFlag(Flags.IndentWithTabs, indentWithTabs.Value);
        }

        if (suppressChecksum.HasValue)
        {
            flags.UpdateFlag(Flags.SuppressChecksum, suppressChecksum.Value);
        }

        if (suppressMetadataAttributes.HasValue)
        {
            flags.UpdateFlag(Flags.SuppressMetadataAttributes, suppressMetadataAttributes.Value);
        }

        if (suppressMetadataSourceChecksumAttributes.HasValue)
        {
            flags.UpdateFlag(Flags.SuppressMetadataSourceChecksumAttributes, suppressMetadataSourceChecksumAttributes.Value);
        }

        if (suppressPrimaryMethodBody.HasValue)
        {
            flags.UpdateFlag(Flags.SuppressPrimaryMethodBody, suppressPrimaryMethodBody.Value);
        }

        if (suppressNullabilityEnforcement.HasValue)
        {
            flags.UpdateFlag(Flags.SuppressNullabilityEnforcement, suppressNullabilityEnforcement.Value);
        }

        if (omitMinimizedComponentAttributeValues.HasValue)
        {
            flags.UpdateFlag(Flags.OmitMinimizedComponentAttributeValues, omitMinimizedComponentAttributeValues.Value);
        }

        if (supportLocalizedComponentNames.HasValue)
        {
            flags.UpdateFlag(Flags.SupportLocalizedComponentNames, supportLocalizedComponentNames.Value);
        }

        if (useEnhancedLinePragma.HasValue)
        {
            flags.UpdateFlag(Flags.UseEnhancedLinePragma, useEnhancedLinePragma.Value);
        }

        if (suppressAddComponentParameter.HasValue)
        {
            flags.UpdateFlag(Flags.SuppressAddComponentParameter, suppressAddComponentParameter.Value);
        }

        if (remapLinePragmaPathsOnWindows.HasValue)
        {
            flags.UpdateFlag(Flags.RemapLinePragmaPathsOnWindows, remapLinePragmaPathsOnWindows.Value);
        }

        return flags == _flags
            ? this
            : new(LanguageVersion, FileKind, IndentSize, NewLine, RootNamespace, SuppressUniqueIds, flags);
    }
}
