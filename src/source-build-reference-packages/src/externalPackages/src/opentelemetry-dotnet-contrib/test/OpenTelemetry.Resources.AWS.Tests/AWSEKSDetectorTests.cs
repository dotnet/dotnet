// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests;

public class AWSEKSDetectorTests
{
    private const string AWSEKSCredentialsPath = "SampleMetadataFiles/testekstoken";
    private const string AWSEKSMetadataFilePath = "SampleMetadataFiles/testcgroup";

    [Fact]
    public void TestDetect()
    {
        var eksResourceDetector = new AWSEKSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var resourceAttributes = eksResourceDetector.Detect();

        Assert.NotNull(resourceAttributes);
        Assert.Empty(resourceAttributes.Attributes);
    }

    [Fact]
    public void TestExtractResourceAttributes()
    {
        var clusterName = "Test cluster name";
        var containerId = "Test container id";

        var eksResourceDetector = new AWSEKSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var resourceAttributes = eksResourceDetector.ExtractResourceAttributes(clusterName, containerId).ToDictionary(x => x.Key, x => x.Value);

        Assert.Equal(4, resourceAttributes.Count);
        Assert.Equal("aws", resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("aws_eks", resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("Test cluster name", resourceAttributes[ExpectedSemanticConventions.AttributeK8SClusterName]);
        Assert.Equal("Test container id", resourceAttributes[ExpectedSemanticConventions.AttributeContainerID]);
    }

    [Fact]
    public void TestExtractResourceAttributesWithEmptyClusterName()
    {
        var containerId = "Test container id";

        var eksResourceDetector = new AWSEKSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var resourceAttributes = eksResourceDetector.ExtractResourceAttributes(string.Empty, containerId).ToDictionary(x => x.Key, x => x.Value);

        // Validate the count of resourceAttributes -> Excluding cluster name, there will be only three resourceAttributes
        Assert.Equal(3, resourceAttributes.Count);
        Assert.Equal("aws", resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("aws_eks", resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("Test container id", resourceAttributes[ExpectedSemanticConventions.AttributeContainerID]);
    }

    [Fact]
    public void TestExtractResourceAttributesWithEmptyContainerId()
    {
        var clusterName = "Test cluster name";

        var eksResourceDetector = new AWSEKSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var resourceAttributes = eksResourceDetector.ExtractResourceAttributes(clusterName, string.Empty).ToDictionary(x => x.Key, x => x.Value);

        // Validate the count of resourceAttributes -> Excluding container id, there will be only three resourceAttributes
        Assert.Equal(3, resourceAttributes.Count);
        Assert.Equal("aws", resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("aws_eks", resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("Test cluster name", resourceAttributes[ExpectedSemanticConventions.AttributeK8SClusterName]);
    }

    [Fact]
    public void TestGetEKSCredentials()
    {
        var eksCredentials = AWSEKSDetector.GetEKSCredentials(AWSEKSCredentialsPath);

        Assert.Equal("Bearer Test AWS EKS Token", eksCredentials);
    }

    [Fact]
    public void TestGetEKSContainerId()
    {
        var eksContainerId = AWSEKSDetector.GetEKSContainerId(AWSEKSMetadataFilePath);

        Assert.Equal("a4d00c9dd675d67f866c786181419e1b44832d4696780152e61afd44a3e02856", eksContainerId);
    }

    [Fact]
    public void TestDeserializeResponse()
    {
        var awsEKSClusterInformation = "{\"kind\": \"ConfigMap\", \"apiVersion\": \"v1\", \"metadata\": {\"name\": \"cluster-info\", \"namespace\": \"amazon-cloudwatch\", \"selfLink\": \"/api/v1/namespaces/amazon-cloudwatch/configmaps/cluster-info\", \"uid\": \"0734438c-48f4-45c3-b06d-b6f16f7f0e1e\", \"resourceVersion\": \"25911\", \"creationTimestamp\": \"2021-07-23T18:41:56Z\", \"annotations\": {\"kubectl.kubernetes.io/last-applied-configuration\": \"{\\\"apiVersion\\\":\\\"v1\\\",\\\"data\\\":{\\\"cluster.name\\\":\\\"Test\\\",\\\"logs.region\\\":\\\"us-west-2\\\"},\\\"kind\\\":\\\"ConfigMap\\\",\\\"metadata\\\":{\\\"annotations\\\":{},\\\"name\\\":\\\"cluster-info\\\",\\\"namespace\\\":\\\"amazon-cloudwatch\\\"}}\\n\"}}, \"data\": {\"cluster.name\": \"Test\", \"logs.region\": \"us-west-2\"}}";

        var eksClusterInformation = AWSEKSDetector.DeserializeResponse(awsEKSClusterInformation);

        Assert.NotNull(eksClusterInformation);
        Assert.NotNull(eksClusterInformation.Data);
        Assert.Equal("Test", eksClusterInformation.Data.ClusterName);
    }

    private static class ExpectedSemanticConventions
    {
        public const string AttributeCloudProvider = "cloud.provider";
        public const string AttributeCloudPlatform = "cloud.platform";
        public const string AttributeK8SClusterName = "k8s.cluster.name";
        public const string AttributeContainerID = "container.id";
    }
}

#endif
