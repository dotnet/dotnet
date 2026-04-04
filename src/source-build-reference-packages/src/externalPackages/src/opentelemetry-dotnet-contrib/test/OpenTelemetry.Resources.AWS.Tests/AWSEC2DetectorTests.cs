// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif
using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests;

public class AWSEC2DetectorTests
{
    private const string AwsEc2InstanceIdUrl = "http://169.254.169.254/latest/meta-data/instance-id";
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(3),
    };

    public static async Task<bool> IsRunningOnEC2()
    {
        try
        {
            var instanceId = await HttpClient.GetStringAsync(new Uri(AwsEc2InstanceIdUrl));
            return !string.IsNullOrEmpty(instanceId);
        }
        catch
        {
            return false;
        }
    }

    [Fact]
    public async Task TestDetect()
    {
        var awsEC2Detector = new AWSEC2Detector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var attributes = awsEC2Detector.Detect().Attributes;
        if (!await IsRunningOnEC2())
        {
            Assert.Empty(attributes); // will be null as it's not in ec2 environment
        }
        else
        {
            Assert.NotEmpty(attributes);
        }
    }

    [Fact]
    public void TestExtractResourceAttributes()
    {
        var awsEC2Detector = new AWSEC2Detector(
            new OpenTelemetry.AWS.AWSSemanticConventions(
                SemanticConventionVersion.Latest));

        var sampleEC2IdentityDocumentModel = new SampleAWSEC2IdentityDocumentModel();
        var hostName = "Test host name";
        var resourceAttributes = awsEC2Detector.ExtractResourceAttributes(sampleEC2IdentityDocumentModel, hostName).ToDictionary(x => x.Key, x => x.Value);

        Assert.Equal("aws", resourceAttributes[ExpectedSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("aws_ec2", resourceAttributes[ExpectedSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("Test account id", resourceAttributes[ExpectedSemanticConventions.AttributeCloudAccountID]);
        Assert.Equal("Test availability zone", resourceAttributes[ExpectedSemanticConventions.AttributeCloudAvailabilityZone]);
        Assert.Equal("Test instance id", resourceAttributes[ExpectedSemanticConventions.AttributeHostID]);
        Assert.Equal("Test instance type", resourceAttributes[ExpectedSemanticConventions.AttributeHostType]);
        Assert.Equal("Test aws region", resourceAttributes[ExpectedSemanticConventions.AttributeCloudRegion]);
        Assert.Equal("Test host name", resourceAttributes[ExpectedSemanticConventions.AttributeHostName]);
    }

    [Fact]
    public void TestDeserializeResponse()
    {
        var ec2IdentityDocument = "{\"accountId\": \"123456789012\", \"architecture\": \"x86_64\", \"availabilityZone\": \"us-east-1a\", \"billingProducts\": null, \"devpayProductCodes\": null, \"marketplaceProductCodes\": null, \"imageId\": \"ami-12345678901234567\", \"instanceId\": \"i-12345678901234567\", \"instanceType\": \"t2.micro\", \"kernelId\": null, \"pendingTime\": \"2021-08-11T22:41:54Z\", \"privateIp\": \"123.456.789.123\", \"ramdiskId\": null, \"region\": \"us-east-1\", \"version\": \"2021-08-11\"}";

        var ec2IdentityDocumentModel = AWSEC2Detector.DeserializeResponse(ec2IdentityDocument);

        Assert.NotNull(ec2IdentityDocumentModel);
        Assert.Equal("123456789012", ec2IdentityDocumentModel.AccountId);
        Assert.Equal("us-east-1a", ec2IdentityDocumentModel.AvailabilityZone);
        Assert.Equal("i-12345678901234567", ec2IdentityDocumentModel.InstanceId);
        Assert.Equal("t2.micro", ec2IdentityDocumentModel.InstanceType);
        Assert.Equal("us-east-1", ec2IdentityDocumentModel.Region);
    }

    private static class ExpectedSemanticConventions
    {
        public const string AttributeCloudProvider = "cloud.provider";
        public const string AttributeCloudPlatform = "cloud.platform";
        public const string AttributeCloudAccountID = "cloud.account.id";
        public const string AttributeCloudAvailabilityZone = "cloud.availability_zone";
        public const string AttributeCloudRegion = "cloud.region";
        public const string AttributeHostID = "host.id";
        public const string AttributeHostType = "host.type";
        public const string AttributeHostName = "host.name";
    }
}
