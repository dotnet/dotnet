// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetConsole
{
    /// <summary>
    /// Host MEF metadata viewer.
    /// </summary>
    public interface IHostMetadata
    {
        /// <summary>
        /// Get the HostName MEF metadata.
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// Get the DisplayName MEF metadata.
        /// </summary>
        string DisplayName { get; }
    }
}
