// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class RazorCodeGenerationOptions(
    RazorCodeGenerationOptionsFlags flags,
    int indentSize,
    string newLine,
    string? rootNamespace,
    string? suppressUniqueIds)
{
    private readonly RazorCodeGenerationOptionsFlags _flags = flags;

    public bool DesignTime => _flags.HasFlag(RazorCodeGenerationOptionsFlags.DesignTime);

    public bool IndentWithTabs => _flags.HasFlag(RazorCodeGenerationOptionsFlags.IndentWithTabs);
    public int IndentSize { get; } = indentSize;
    public string NewLine { get; } = newLine;

    /// <summary>
    /// Gets the root namespace for the generated code.
    /// </summary>
    public string? RootNamespace { get; } = rootNamespace;

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
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SuppressChecksum);

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
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SuppressMetadataAttributes);

    /// <summary>
    /// Gets a value that indicates whether to suppress the <c>RazorSourceChecksumAttribute</c>.
    /// <para>
    /// Used by default in .NET 6 apps since including a type-level attribute that changes on every
    /// edit are treated as rude edits by hot reload.
    /// </para>
    /// </summary>
    public bool SuppressMetadataSourceChecksumAttributes
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SuppressMetadataSourceChecksumAttributes);

    /// <summary>
    /// Gets or sets a value that determines if an empty body is generated for the primary method.
    /// </summary>
    public bool SuppressPrimaryMethodBody
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SuppressPrimaryMethodBody);

    /// <summary>
    /// Gets a value that determines if nullability type enforcement should be suppressed for user code.
    /// </summary>
    public bool SuppressNullabilityEnforcement
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SuppressNullabilityEnforcement);

    /// <summary>
    /// Gets a value that determines if the components code writer may omit values for minimized attributes.
    /// </summary>
    public bool OmitMinimizedComponentAttributeValues
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.OmitMinimizedComponentAttributeValues);

    /// <summary>
    /// Gets a value that determines if localized component names are to be supported.
    /// </summary>
    public bool SupportLocalizedComponentNames
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SupportLocalizedComponentNames);

    /// <summary>
    /// Gets a value that determines if enhanced line pragmas are to be utilized.
    /// </summary>
    public bool UseEnhancedLinePragma
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.UseEnhancedLinePragma);

    /// <summary>
    /// Gets a value used for unique ids for testing purposes. Null for unique ids.
    /// </summary>
    public string? SuppressUniqueIds { get; } = suppressUniqueIds;

    /// <summary>
    /// Determines whether RenderTreeBuilder.AddComponentParameter should not be used.
    /// </summary>
    public bool SuppressAddComponentParameter
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.SuppressAddComponentParameter);

    /// <summary>
    /// Determines if the file paths emitted as part of line pragmas should be mapped back to a valid path on windows.
    /// </summary>
    public bool RemapLinePragmaPathsOnWindows
        => _flags.HasFlag(RazorCodeGenerationOptionsFlags.RemapLinePragmaPathsOnWindows);

    public static RazorCodeGenerationOptions Default { get; } = new RazorCodeGenerationOptions(
        flags: RazorCodeGenerationOptionsFlags.DefaultFlags,
        indentSize: 4,
        newLine: Environment.NewLine,
        rootNamespace: null,
        suppressUniqueIds: null);

    public static RazorCodeGenerationOptions DesignTimeDefault { get; } = new RazorCodeGenerationOptions(
        flags: RazorCodeGenerationOptionsFlags.DefaultDesignTimeFlags,
        indentSize: 4,
        newLine: Environment.NewLine,
        rootNamespace: null,
        suppressUniqueIds: null);

    public static RazorCodeGenerationOptions Create(Action<RazorCodeGenerationOptionsBuilder> configure)
    {
        ArgHelper.ThrowIfNull(configure);

        var builder = new RazorCodeGenerationOptionsBuilder(designTime: false);
        configure(builder);
        var options = builder.Build();

        return options;
    }

    public static RazorCodeGenerationOptions CreateDesignTime(Action<RazorCodeGenerationOptionsBuilder> configure)
    {
        ArgHelper.ThrowIfNull(configure);

        var builder = new RazorCodeGenerationOptionsBuilder(designTime: true)
        {
            SuppressMetadataAttributes = true,
        };

        configure(builder);
        var options = builder.Build();

        return options;
    }

    public RazorCodeGenerationOptionsBuilder ToBuilder()
    {
        var builder = new RazorCodeGenerationOptionsBuilder(_flags)
        {
            IndentSize = IndentSize,
            NewLine = NewLine,
            RootNamespace = RootNamespace,
            SuppressUniqueIds = SuppressUniqueIds
        };

        return builder;
    }
}
