// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// State of a plugin file path.
    /// </summary>
    public enum PluginFileState
    {
        /// <summary>
        /// The file exists and has a valid embedded signature.
        /// </summary>
        Valid,

        /// <summary>
        /// The file was not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// The file path was invalid (e.g.:  not rooted).
        /// </summary>
        InvalidFilePath,

        /// <summary>
        /// The file exists but it has either no embedded signature or an invalid embedded signature.
        /// </summary>
        /// <remarks>No longer used.</remarks>
        InvalidEmbeddedSignature
    }
}
