// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Credentials
{
    /// <summary>
    /// Request data passed to plugin credential provider applications.
    /// </summary>
    public class PluginCredentialRequest
    {
        /// <summary>
        /// Gets or sets the package source URI for the credential request.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request is non-interactive.
        /// </summary>
        public bool NonInteractive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request is a retry.
        /// </summary>
        public bool IsRetry { get; set; }

        /// <summary>
        /// Gets or sets the verbosity level for the request.
        /// </summary>
        public string Verbosity { get; set; }
    }
}
