// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGetConsole
{
    /// <summary>
    /// Service for creating and working with new IWpfConsole. This is for hosts that
    /// create and manage their own command window with custom editor ContentType.
    /// </summary>
    public interface IWpfConsoleService
    {
        /// <summary>
        /// Create a new IWpfConsole.
        /// </summary>
        /// <param name="sp">An IServiceProvider used by the new IWpfConsole.</param>
        /// <param name="contentTypeName">The editor ContentType used by the new IWpfConsole.</param>
        /// <param name="hostName">The HostName identity used by the new IWpfConsole.</param>
        IWpfConsole CreateConsole(IServiceProvider sp, string contentTypeName, string hostName);

        /// <summary>
        /// TryCreate an ICompletionSource for a textBuffer associated with an IWpfConsole.
        /// This is for implementing ICompletionSourceProvider for custom ContentType.
        /// </summary>
        /// <param name="textBuffer">The ITextBuffer object.</param>
        /// <returns>An ICompletionSource object.</returns>
        object TryCreateCompletionSource(object textBuffer);

        /// <summary>
        /// Get an IClassifier for a text buffer associated with an IWpfConsole.
        /// This is for implementing IClassifierProvider for custom ContentType.
        /// </summary>
        /// <param name="textBuffer">The ITextBuffer object.</param>
        /// <returns>An IClassifier object.</returns>
        object GetClassifier(object textBuffer);
    }
}
