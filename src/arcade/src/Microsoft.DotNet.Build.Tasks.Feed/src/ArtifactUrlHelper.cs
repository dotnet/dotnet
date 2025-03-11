// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Feed;

using System;

/// <summary>
/// Interface for constructing download URLs for artifacts.
/// </summary>
public interface IArtifactUrlHelper
{
    /// <summary>
    /// Construct the download URI for the given file name.
    /// </summary>
    string ConstructDownloadUrl(string fileName);
}

public class BuildArtifactUrlHelper : IArtifactUrlHelper
{
    private readonly string _containerId;
    private readonly string _artifactName;
    private readonly string _azureDevOpsBaseUrl;
    private readonly string _azureDevOpsOrg;
    private readonly string _apiVersionForFileDownload;

    public BuildArtifactUrlHelper(
        string containerId,
        string artifactName,
        string azureDevOpsBaseUrl,
        string azureDevOpsOrg,
        string apiVersionForFileDownload)
    {
        _containerId = containerId;
        _artifactName = artifactName;
        _azureDevOpsBaseUrl = azureDevOpsBaseUrl;
        _azureDevOpsOrg = azureDevOpsOrg;
        _apiVersionForFileDownload = apiVersionForFileDownload;
    }

    public string ConstructDownloadUrl(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        // Construct the download URL for the artifact.
        // Of the format:
        // escapedItemPath = escaped(itemPath=<artifactName>/<fileName>)
        // $"{AzureDevOpsBaseUrl}/{AzureDevOpsOrg}/_apis/resources/Containers/{containerId}?itemPath=/{artifactName}/{fileName}&isShallow=true&api-version={AzureApiVersionForFileDownload}";
        string itemPath = Uri.EscapeDataString($"{_artifactName}/{fileName}");
        return $"{_azureDevOpsBaseUrl}/{_azureDevOpsOrg}/_apis/resources/Containers/{_containerId}?itemPath={itemPath}&isShallow=true&api-version={_apiVersionForFileDownload}";
    }
}

public class PipelineArtifactDownloadHelper : IArtifactUrlHelper
{
    private readonly string _baseUrl;

    public PipelineArtifactDownloadHelper(string baseUrl)
    {
        // Construct the incoming URI, which is of the form:
        // https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip
        // and remove the query string.
        var uriBuilder = new UriBuilder(baseUrl);
        uriBuilder.Query = string.Empty;
        _baseUrl = uriBuilder.ToString();
    }

    public string ConstructDownloadUrl(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }
        // If the file name is 
        var uriBuilder = new UriBuilder(_baseUrl);
        string subPath = Uri.EscapeDataString("/" + fileName);
        uriBuilder.Query = $"?format=file&subPath={subPath}";
        return uriBuilder.Uri.ToString();
    }
}
