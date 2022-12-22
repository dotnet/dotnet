// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <inheritdoc/>
public class PlatformAssemblyResolver : IAssemblyResolver
{
    /// <summary>
    /// Specifies whether the resolver is disposed or not
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformAssemblyResolver"/> class.
    /// </summary>
    /// <param name="directories"> The search directories. </param>
    public PlatformAssemblyResolver()
    {
        AssemblyLoadContext.Default.Resolving += AssemblyResolverEvent;
    }

    ~PlatformAssemblyResolver()
    {
        Dispose(false);
    }

    /// <inheritdoc/>
    public event AssemblyResolveEventHandler? AssemblyResolve;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            AssemblyLoadContext.Default.Resolving -= AssemblyResolverEvent;
        }

        _disposed = true;
    }

    /// <summary>
    /// Assembly Resolve event handler for App Domain - called when CLR loader cannot resolve assembly.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="eventArgs">
    /// The event Args.
    /// </param>
    /// <returns>
    /// The <see cref="Assembly"/>.
    /// </returns>
    private Assembly? AssemblyResolverEvent(object sender, object eventArgs)
    {
        return eventArgs is not AssemblyName args ? null : AssemblyResolve?.Invoke(this, new AssemblyResolveEventArgs(args.Name));
    }
}

#endif
