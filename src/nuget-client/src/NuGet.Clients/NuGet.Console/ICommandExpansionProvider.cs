// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.VisualStudio;

namespace NuGetConsole
{
    /// <summary>
    /// ICommandExpansion provider (intellisense).
    /// To support command expansion, export an implementation of this interface
    /// and apply a HostName attribute to associate it with the host.
    /// </summary>
    public interface ICommandExpansionProvider
    {
        /// <summary>
        /// Create a command line expansion object.
        /// </summary>
        /// <param name="host">The host instance for command line expansion.</param>
        /// <returns>A command line expansion object.</returns>
        ICommandExpansion Create(IHost host);
    }
}
