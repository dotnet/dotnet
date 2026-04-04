// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests;

public class AWSEBSDetectorTests
{
    private const string AWSEBSMetadataFilePath = "SampleMetadataFiles/environment.conf";

    [Fact]
    public void TestDetect()
    {
        var awsEBSDetector = new AWSEBSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        Assert.Empty(awsEBSDetector.Detect().Attributes); // will be null as it's not in ebs environment
    }

    [Fact]
    public void TestExtractResourceAttributes()
    {
        var awsEBSDetector = new AWSEBSDetector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var sampleModel = new SampleAWSEBSMetadataModel();

        var resourceAttributes = awsEBSDetector.ExtractResourceAttributes(sampleModel).ToDictionary(x => x.Key, x => x.Value);

        Assert.Equal("aws", resourceAttributes[AWSSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("aws_elastic_beanstalk", resourceAttributes[AWSSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("aws_elastic_beanstalk", resourceAttributes[AWSSemanticConventions.AttributeServiceName]);
        Assert.Equal("Test environment name", resourceAttributes[AWSSemanticConventions.AttributeServiceNamespace]);
        Assert.Equal("Test ID", resourceAttributes[AWSSemanticConventions.AttributeServiceInstanceID]);
        Assert.Equal("Test version label", resourceAttributes[AWSSemanticConventions.AttributeServiceVersion]);
    }

    [Fact]
    public void TestGetEBSMetadata()
    {
        var ebsMetadata = AWSEBSDetector.GetEBSMetadata(AWSEBSMetadataFilePath);

        Assert.NotNull(ebsMetadata);
        Assert.Equal("1234567890", ebsMetadata.DeploymentId);
        Assert.Equal("Test AWS Elastic Beanstalk Environment Name", ebsMetadata.EnvironmentName);
        Assert.Equal("Test Version", ebsMetadata.VersionLabel);
    }

    private static class AWSSemanticConventions
    {
        public const string AttributeCloudProvider = "cloud.provider";
        public const string AttributeCloudPlatform = "cloud.platform";
        public const string AttributeServiceName = "service.name";
        public const string AttributeServiceNamespace = "service.namespace";
        public const string AttributeServiceInstanceID = "service.instance.id";
        public const string AttributeServiceVersion = "service.version";
    }
}
