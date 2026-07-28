// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.Configuration;

/// <summary>
/// Defines the context for configuration retrieval operations.
/// </summary>
public class ConfigurationRetrievalContext
{
    /// <summary>
    /// Gets or sets a value indicating whether to bypass the cache when retrieving configuration.
    /// </summary>
    public bool BypassCache { get; set; }
}
