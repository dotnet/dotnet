// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// ExperimentalAttribute ships in the framework from .NET 8 onwards. The assemblies that apply
// [Experimental] also build target frameworks that predate it, so it is declared here for those
// legs. The compiler binds the attribute by full name rather than by identity, so an internal
// declaration in the right namespace is enough for the VSTEST001 diagnostic to be reported.
#if !NET8_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Indicates that an API is experimental and it may change in the future.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly
    | AttributeTargets.Module
    | AttributeTargets.Class
    | AttributeTargets.Struct
    | AttributeTargets.Enum
    | AttributeTargets.Constructor
    | AttributeTargets.Method
    | AttributeTargets.Property
    | AttributeTargets.Field
    | AttributeTargets.Event
    | AttributeTargets.Interface
    | AttributeTargets.Delegate, Inherited = false)]
internal sealed class ExperimentalAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExperimentalAttribute"/> class, specifying the ID that the compiler will use
    /// when reporting a use of the API the attribute applies to.
    /// </summary>
    /// <param name="diagnosticId">The ID that the compiler will use when reporting a use of the API the attribute applies to.</param>
    public ExperimentalAttribute(string diagnosticId)
    {
        DiagnosticId = diagnosticId;
    }

    /// <summary>
    /// Gets the ID that the compiler will use when reporting a use of the API the attribute applies to.
    /// </summary>
    public string DiagnosticId { get; }

    /// <summary>
    /// Gets or sets the URL for corresponding documentation.
    /// </summary>
    public string? UrlFormat { get; set; }
}

#endif
