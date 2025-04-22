// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using FluentAssertions;
using Xunit;
using Microsoft.DotNet.Build.Tasks.Feed;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests;

public class ArtifactUrlHelperTests
{
    [Theory]
    [InlineData("12345", "artifact", "https://dev.azure.com", "myOrg", "6.0", "file.txt", "https://dev.azure.com/myOrg/_apis/resources/Containers/12345?itemPath=artifact%2Ffile.txt&isShallow=true&api-version=6.0")]
    [InlineData("12345", "artifact", "https://dev.azure.com", "myOrg", "6.0", "subdir/file.txt", "https://dev.azure.com/myOrg/_apis/resources/Containers/12345?itemPath=artifact%2Fsubdir%2Ffile.txt&isShallow=true&api-version=6.0")]
    public void BuildArtifactUrlHelper_ConstructDownloadUrl_ReturnsCorrectUrl(
        string containerId, string artifactName, string azureDevOpsBaseUrl, string azureDevOpsOrg, string apiVersionForFileDownload, string fileName, string expectedUrl)
    {
        // Arrange
        var helper = new BuildArtifactUrlHelper(containerId, artifactName, azureDevOpsBaseUrl, azureDevOpsOrg, apiVersionForFileDownload);

        // Act
        string result = helper.ConstructDownloadUrl(fileName);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("12345", "artifact", "https://dev.azure.com", "myOrg", "6.0", "")]
    [InlineData("12345", "artifact", "https://dev.azure.com", "myOrg", "6.0", null)]
    public void BuildArtifactUrlHelper_ConstructDownloadUrl_ThrowsArgumentException(
        string containerId, string artifactName, string azureDevOpsBaseUrl, string azureDevOpsOrg, string apiVersionForFileDownload, string fileName)
    {
        // Arrange
        var helper = new BuildArtifactUrlHelper(containerId, artifactName, azureDevOpsBaseUrl, azureDevOpsOrg, apiVersionForFileDownload);

        // Act
        Action act = () => helper.ConstructDownloadUrl(fileName);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("File name cannot be null or empty. (Parameter 'fileName')");
    }

    [Theory]
    [InlineData("https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip", "file.txt", "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=file&subPath=%2Ffile.txt")]
    [InlineData("https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip", "subdir/file.txt", "https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=file&subPath=%2Fsubdir%2Ffile.txt")]
    public void PipelineArtifactDownloadHelper_ConstructDownloadUrl_ReturnsCorrectUrl(
        string baseUrl, string fileName, string expectedUrl)
    {
        // Arrange
        var helper = new PipelineArtifactDownloadHelper(baseUrl);

        // Act
        string result = helper.ConstructDownloadUrl(fileName);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip", "")]
    [InlineData("https://artprodcus3.artifacts.visualstudio.com/Ab55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8/7ea9116e-9fac-403d-b258-b31fcf1bb293/_apis/artifact/HASH/content?format=zip", null)]
    public void PipelineArtifactDownloadHelper_ConstructDownloadUrl_ThrowsArgumentException(string baseUrl, string fileName)
    {
        // Arrange
        var helper = new PipelineArtifactDownloadHelper(baseUrl);

        // Act
        Action act = () => helper.ConstructDownloadUrl(fileName);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("File name cannot be null or empty. (Parameter 'fileName')");
    }
}
