// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Message methods.
    /// </summary>
    public enum MessageMethod
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Close
        /// </summary>
        Close,

        /// <summary>
        /// Copy files in a package
        /// </summary>
        CopyFilesInPackage,

        /// <summary>
        /// Copy a .nupkg file
        /// </summary>
        CopyNupkgFile,

        /// <summary>
        /// Get credentials
        /// </summary>
        GetCredentials,

        /// <summary>
        /// Get files in a package
        /// </summary>
        GetFilesInPackage,

        /// <summary>
        /// Get operation claims
        /// </summary>
        GetOperationClaims,

        /// <summary>
        /// Get package hash
        /// </summary>
        GetPackageHash,

        /// <summary>
        /// Get package versions
        /// </summary>
        GetPackageVersions,

        /// <summary>
        /// Get service index
        /// </summary>
        GetServiceIndex,

        /// <summary>
        /// Handshake
        /// </summary>
        Handshake,

        /// <summary>
        /// Initialize
        /// </summary>
        Initialize,

        /// <summary>
        /// Log
        /// </summary>
        Log,

        /// <summary>
        /// Monitor NuGet process exit
        /// </summary>
        MonitorNuGetProcessExit,

        /// <summary>
        /// Prefetch a package
        /// </summary>
        PrefetchPackage,

        /// <summary>
        /// Set credentials
        /// </summary>
        SetCredentials,

        /// <summary>
        /// Set log level
        /// </summary>
        SetLogLevel,

        /// <summary>
        /// Get authentication credentials, for authentication operation
        /// </summary>
        GetAuthenticationCredentials,
    }
}
