// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

/// <summary>
/// Represents a method that handles the AssemblyResolve event of an Platform.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="args">The event data.</param>
/// <returns>The assembly that resolves the type, assembly, or resource; or null if the assembly
/// cannot be resolved.
/// </returns>
public delegate Assembly? AssemblyResolveEventHandler(object? sender, AssemblyResolveEventArgs? args);

/// <summary>
/// The AssemblyResolver interface.
/// </summary>
public interface IAssemblyResolver : IDisposable
{
    /// <summary>
    /// Occurs when the resolution of an assembly fails
    /// </summary>
    event AssemblyResolveEventHandler? AssemblyResolve;
}

/// <summary>
/// Provides data for loader resolution events, such as the AppDomain.AssemblyResolve events.
/// </summary>
public class AssemblyResolveEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyResolveEventArgs"/> class.
    /// name of the item to resolve.
    /// </summary>
    /// <param name="name">The Full name of an assembly to resolve.</param>
    public AssemblyResolveEventArgs(string? name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyResolveEventArgs"/> class.
    /// </summary>
    /// <param name="name">The full name of an assembly to resolve.</param>
    /// <param name="requestingAssembly">The assembly whose dependency is being resolved, or <see langword="null"/> if the requesting assembly is not known.</param>
    public AssemblyResolveEventArgs(string? name, Assembly? requestingAssembly)
    {
        Name = name;
        RequestingAssembly = requestingAssembly;
    }

    /// <summary>
    /// Gets or sets the name of the item to resolve.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the assembly whose dependency is being resolved.
    /// </summary>
    /// <remarks>
    /// This property is populated on .NET Framework (net462+) where <c>AppDomain.AssemblyResolve</c>
    /// exposes the requesting assembly via <see cref="ResolveEventArgs.RequestingAssembly"/>.
    /// On .NET (Core), <c>AssemblyLoadContext.Resolving</c> does not expose the requesting assembly,
    /// so this property is always <see langword="null"/> in that environment.
    /// </remarks>
    public Assembly? RequestingAssembly { get; }
}
