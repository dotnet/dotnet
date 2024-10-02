// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER

#nullable enable

namespace Microsoft.DotNet.ArcadeAzureIntegration;


public class DefaultIdentityTokenCredentialOptions
{
    public string? ManagedIdentityClientId { get; set; } = null;
    public bool ExcludeAzureCliCredential { get; set; }
}

#endif
