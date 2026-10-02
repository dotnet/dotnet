// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using Azure.Core;
using Azure.Identity;
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.DotNet.ArcadeAzureIntegration;


// This implementation of TokenCredential will try to cover all common ways of
// authentication to Azure services used in Arcade tooling
public class DefaultIdentityTokenCredential : TokenCredential
{
    private readonly TokenCredential _tokenCredential;

    public DefaultIdentityTokenCredential()
        : this(new DefaultIdentityTokenCredentialOptions())
    {
    }

    public DefaultIdentityTokenCredential(DefaultIdentityTokenCredentialOptions options)
    {
        _tokenCredential = CreateAvailableTokenCredential(options);
    }

    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return _tokenCredential.GetTokenAsync(requestContext, cancellationToken);
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return _tokenCredential.GetToken(requestContext, cancellationToken);
    }

    private static TokenCredential CreateAvailableTokenCredential(DefaultIdentityTokenCredentialOptions options)
    {
        TokenCredential? azurePipelinesCredential = GetAzurePipelinesCredentialForAzurePipelineTask();

        if (options.UseAzurePipelineCredentialAloneIfConfigured && !options.PreferAzureCliCredential)
        {
            if (azurePipelinesCredential != null)
            {
                if (!options.DisableShortCache)
                {
                    return new TokenCredentialShortCache(azurePipelinesCredential);
                }
                return azurePipelinesCredential;
            }
        }

        List<TokenCredential> tokenCredentials = [];

        TokenCredential? azureCliCredential = options.ExcludeAzureCliCredential
            ? null
            : CreateAzureCliCredential(options.DisableShortCache);

        if (options.PreferAzureCliCredential && azureCliCredential != null)
        {
            tokenCredentials.Add(azureCliCredential);
        }

        // Add Azure Pipelines credential if the environment variables are set
        if (azurePipelinesCredential != null)
        {
            if (!options.DisableShortCache)
            {
                azurePipelinesCredential = new TokenCredentialShortCache(azurePipelinesCredential);
            }
            tokenCredentials.Add(azurePipelinesCredential);
        }

        // Add Managed Identity credential
        ManagedIdentityId managedIdentityId = string.IsNullOrEmpty(options.ManagedIdentityClientId)
            ? ManagedIdentityId.SystemAssigned
            : ManagedIdentityId.FromUserAssignedClientId(options.ManagedIdentityClientId);
        tokenCredentials.Add(new ManagedIdentityCredential(managedIdentityId));

        // Add work load identity credential if the environment variables are set
        TokenCredential? workloadIdentityCredential = GetWorkloadIdentityCredentialForAzurePipelineTask();
        if (workloadIdentityCredential != null)
        {
            if (!options.DisableShortCache)
            {
                workloadIdentityCredential = new TokenCredentialShortCache(workloadIdentityCredential);
            }
            tokenCredentials.Add(workloadIdentityCredential);
        }

        if (!options.PreferAzureCliCredential && azureCliCredential != null)
        {
            tokenCredentials.Add(azureCliCredential);
        }

        if (tokenCredentials.Count == 0)
        {
            throw new InvalidOperationException("No valid credential class detected and configured for authentication to Azure services.");
        }

        var ret = new ChainedTokenCredential(tokenCredentials.ToArray());
        return ret;
    }

    private static TokenCredential CreateAzureCliCredential(bool disableShortCache)
    {
        TokenCredential credential = new AzureCliCredentialWithAzNoUpdateWrapper(
            new AzureCliCredential(new AzureCliCredentialOptions
            {
                ProcessTimeout = TimeSpan.FromSeconds(30)
            }));

        return disableShortCache
            ? credential
            : new TokenCredentialShortCache(credential);
    }

    private static object _workloadTokenFileLock = new object();
    private static string? _workloadTokenFile = null;
    private static string? _workloadToken = null;

    // Create WorkloadIdentityCredential if the environment variables set by AzurePipeline are provided
    private static WorkloadIdentityCredential? GetWorkloadIdentityCredentialForAzurePipelineTask()
    {
        string? servicePrincipalId = Environment.GetEnvironmentVariable("servicePrincipalId");
        string? idToken = Environment.GetEnvironmentVariable("idToken");
        string? tenantId = Environment.GetEnvironmentVariable("tenantId");

        if (!string.IsNullOrEmpty(idToken) &&
            !string.IsNullOrEmpty(tenantId) &&
            !string.IsNullOrEmpty(servicePrincipalId))
        {
            lock (_workloadTokenFileLock)
            {
                if (idToken != _workloadToken)
                {
                    // create token file
                    var tokenFileName = Path.GetTempFileName();
                    File.WriteAllText(tokenFileName, idToken);
                    _workloadTokenFile = tokenFileName;
                    _workloadToken = idToken;
                }
                return new WorkloadIdentityCredential(new WorkloadIdentityCredentialOptions
                {
                    ClientId = servicePrincipalId,
                    TokenFilePath = _workloadTokenFile,
                    TenantId = tenantId,
                });
            }
        }
        return null;
    }

    // Create AzurePipelinesCredential if the environment variables set by AzureCli task and SYSTEM_ACCESSTOKEN are provided
    private static AzurePipelinesCredential? GetAzurePipelinesCredentialForAzurePipelineTask()
    {
        string? systemAccessToken = Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");
        string? clientId = Environment.GetEnvironmentVariable("AZURESUBSCRIPTION_CLIENT_ID");
        string? tenantId = Environment.GetEnvironmentVariable("AZURESUBSCRIPTION_TENANT_ID");
        string? serviceConnectionId = Environment.GetEnvironmentVariable("AZURESUBSCRIPTION_SERVICE_CONNECTION_ID");

        if (!string.IsNullOrEmpty(systemAccessToken) &&
            !string.IsNullOrEmpty(clientId) &&
            !string.IsNullOrEmpty(tenantId) &&
            !string.IsNullOrEmpty(serviceConnectionId) &&
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SYSTEM_OIDCREQUESTURI")))
        {
            return new AzurePipelinesCredential(tenantId, clientId, serviceConnectionId, systemAccessToken);
        }
        return null;
    }
}
