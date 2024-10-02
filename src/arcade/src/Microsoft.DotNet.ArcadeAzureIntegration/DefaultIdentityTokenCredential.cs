// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using Azure.Core;
using Azure.Identity;

namespace Microsoft.DotNet.ArcadeAzureIntegration;


// This implementation of TokenCredential will try to cover all common ways of
// authentication to Azure services used in Arcade tooling
public class DefaultIdentityTokenCredential : ChainedTokenCredential
{
    public DefaultIdentityTokenCredential()
        : this(new DefaultIdentityTokenCredentialOptions())
    {
    }

    public DefaultIdentityTokenCredential(DefaultIdentityTokenCredentialOptions options)
        : base(CreateAvailableTokenCredentials(options))
    {
    }

    private static TokenCredential[] CreateAvailableTokenCredentials(DefaultIdentityTokenCredentialOptions options)
    {
        List<TokenCredential> tokenCredentials = [];

        // Add Managed Identity credential if the client id is provided
        if (!string.IsNullOrEmpty(options.ManagedIdentityClientId))
        {
            tokenCredentials.Add(
                new ManagedIdentityCredential(options.ManagedIdentityClientId)
            );
        }

        // Add work load identity credential if the environment variables are set
        var workloadIdentityCredential = GetWorkloadIdentityCredentialForAzurePipelineTask();
        if (workloadIdentityCredential != null)
        {
            tokenCredentials.Add(workloadIdentityCredential);
        }

        // Add Azure Pipelines credential if the environment variables are set
        var azurePipelinesCredential = GetAzurePipelinesCredentialForAzurePipelineTask();
        if (azurePipelinesCredential != null)
        {
            tokenCredentials.Add(azurePipelinesCredential);
        }

        if (!options.ExcludeAzureCliCredential)
        {
            // Add Azure CLI credential as the last resort
            // az command to disable auto update of the Azure CLI to avoid timeout waiting for
            // console input will be called before first use of AzureCliCredential
            tokenCredentials.Add(
                new AzureCliCredentialWithAzNoUpdateWrapper(
                    new AzureCliCredential(
                        new AzureCliCredentialOptions
                        {
                            ProcessTimeout = TimeSpan.FromSeconds(30)
                        }
                    )
                )
             );
        }

        if (tokenCredentials.Count == 0)
        {
            throw new InvalidOperationException("No valid credential class detected and configured for authentication to Azure services.");
        }

        return tokenCredentials.ToArray();
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
            !string.IsNullOrEmpty(serviceConnectionId))
        {
            return new AzurePipelinesCredential(tenantId, clientId, serviceConnectionId, systemAccessToken);
        }
        return null;
    }
}

#endif
