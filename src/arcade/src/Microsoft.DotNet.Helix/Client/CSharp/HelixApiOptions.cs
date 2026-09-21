// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Core;
using Azure.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Helix.Client;

public enum HelixApiAuthenticationMode
{
    Anonymous,
    PersonalAccessToken,
    EntraId,
}

partial class HelixApiOptions
{
    public const string ProductionScope = "api://eb70c40b-c265-44f7-842e-1a568f035f33/.default";
    public const string StagingScope = "api://f45b17a4-149b-4f89-91bc-e6331af8d0e8/.default";

    // See https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/core/Azure.Core/src/RetryOptions.cs for values this overrides
    public const int DefaultRetryDelaySeconds = 10;
    public const int DefaultMaxRetryCount = 5;

    public HelixApiOptions(Uri baseUri, TokenCredential credentials, IEnumerable<string> scopes)
    {
        BaseUri = ValidateBaseUri(baseUri);
        Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        if (credentials is HelixApiTokenCredential)
        {
            throw new ArgumentException(
                "Explicit scopes are only supported for Entra credentials. " +
                "For PAT authentication, pass HelixApiTokenCredential without explicit scopes.",
                nameof(credentials));
        }

        string[] tokenScopes = scopes?.ToArray() ?? throw new ArgumentNullException(nameof(scopes));
        if (tokenScopes.Length == 0 || tokenScopes.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("At least one non-empty token scope is required.", nameof(scopes));
        }
        TokenScopes = Array.AsReadOnly(tokenScopes);

        InitializeOptions();
    }

    public HelixApiAuthenticationMode AuthenticationMode { get; private set; }

    public IReadOnlyList<string> TokenScopes { get; private set; } = Array.Empty<string>();

    partial void InitializeOptions()
    {
        if (Credentials == null)
        {
            AuthenticationMode = HelixApiAuthenticationMode.Anonymous;
        }
        else if (Credentials is HelixApiTokenCredential)
        {
            AuthenticationMode = HelixApiAuthenticationMode.PersonalAccessToken;
            TokenScopes = Array.Empty<string>();
            AddPolicy(new HelixApiTokenAuthenticationPolicy(Credentials), HttpPipelinePosition.PerCall);
        }
        else
        {
            AuthenticationMode = HelixApiAuthenticationMode.EntraId;
            if (TokenScopes.Count == 0)
            {
                TokenScopes = Array.AsReadOnly(new[] { GetDefaultScope(BaseUri) });
            }

            AddPolicy(
                new BearerTokenAuthenticationPolicy(Credentials, TokenScopes.ToArray()),
                HttpPipelinePosition.PerRetry);
        }

        // Users should not generally need to modify these but can do so after creating a HelixApi object if needed
        Retry.Delay = TimeSpan.FromSeconds(DefaultRetryDelaySeconds);
        Retry.MaxRetries = DefaultMaxRetryCount;
    }

    private static string GetDefaultScope(Uri baseUri)
    {
        baseUri = ValidateBaseUri(baseUri);

        if (baseUri.Host.Equals("helix.dot.net", StringComparison.OrdinalIgnoreCase))
        {
            return ProductionScope;
        }

        if (baseUri.Host.Equals("helix.int-dot.net", StringComparison.OrdinalIgnoreCase))
        {
            return StagingScope;
        }

        throw new ArgumentException(
            $"No default Entra scope is known for Helix API host '{baseUri.Host}'. " +
            "Use the HelixApiOptions constructor that accepts explicit scopes.",
            nameof(baseUri));
    }

    private static Uri ValidateBaseUri(Uri baseUri)
    {
        if (baseUri == null)
        {
            throw new ArgumentNullException(nameof(baseUri));
        }

        if (!baseUri.IsAbsoluteUri)
        {
            throw new ArgumentException("The Helix API base URI must be absolute.", nameof(baseUri));
        }

        return baseUri;
    }
}
