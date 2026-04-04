// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Api.Gax;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Resources.Gcp.Tests;

public class GcpResourceDetectorTests
{
    [Fact]
    public void TestExtractGkeResourceAttributes()
    {
        var details = new GkePlatformDetails(
            metadataJson: "json",
            projectId: "projectId",
            clusterName: "clusterName",
            location: "location",
            hostName: "hostName",
            instanceId: "instanceId",
            zone: "us-central1-a",
            namespaceId: "namespaceId",
            podId: "podId",
            containerName: "containerName",
            clusterLocation: "clusterLocation");
        var platform = new Platform(details);
        var attrs = GcpResourceDetector.ExtractGkeResourceAttributes(platform).ToDictionary(x => x.Key, x => x.Value);
        Assert.NotNull(attrs);
        Assert.Equal(8, attrs.Count);
        Assert.Equal(ResourceAttributeConstants.GcpCloudProviderValue, attrs[ResourceSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("projectId", attrs[ResourceSemanticConventions.AttributeCloudAccount]);
        Assert.Equal(ResourceAttributeConstants.GcpGkePlatformValue, attrs[ResourceSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("us-central1-a", attrs[ResourceSemanticConventions.AttributeCloudZone]);
        Assert.Equal("instanceId", attrs[ResourceSemanticConventions.AttributeHostId]);
        Assert.Equal("clusterName", attrs[ResourceSemanticConventions.AttributeK8sCluster]);
        Assert.Equal("namespaceId", attrs[ResourceSemanticConventions.AttributeK8sNamespace]);
        Assert.Equal("hostName", attrs[ResourceSemanticConventions.AttributeK8sPod]);
    }

    [Fact]
    public void TestExtractCloudRunResourceAttributes()
    {
        var details = new CloudRunPlatformDetails(
            metadataJson: "json",
            projectId: "projectId",
            zone: "us-central1-a",
            serviceName: "serviceName",
            revisionName: "revisionName",
            configurationName: "configurationName");
        var platform = new Platform(details);
        var attrs = GcpResourceDetector.ExtractCloudRunResourceAttributes(platform).ToDictionary(x => x.Key, x => x.Value);
        Assert.NotNull(attrs);
        Assert.Equal(7, attrs.Count);
        Assert.Equal(ResourceAttributeConstants.GcpCloudProviderValue, attrs[ResourceSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("projectId", attrs[ResourceSemanticConventions.AttributeCloudAccount]);
        Assert.Equal("us-central1-a", attrs[ResourceSemanticConventions.AttributeCloudAvailabilityZone]);
        Assert.Equal(ResourceAttributeConstants.GcpCloudRunPlatformValue, attrs[ResourceSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("us-central1", attrs[ResourceSemanticConventions.AttributeCloudRegion]);
        Assert.Equal("serviceName", attrs[ResourceSemanticConventions.AttributeFaasName]);
        Assert.Equal("revisionName", attrs[ResourceSemanticConventions.AttributeFaasVersion]);
    }

    [Fact]
    public void TestExtractCloudRunResourceAttributesWithInstanceId()
    {
        var cloudRunDetails = new CloudRunPlatformDetails(
            metadataJson: "json",
            projectId: "projectId",
            zone: "us-central1-a",
            serviceName: "serviceName",
            revisionName: "revisionName",
            configurationName: "configurationName");
        var platform = new Platform(cloudRunDetails);
        var gceDetails = new GcePlatformDetails(
            metadataJson: "json",
            projectId: "projectId",
            instanceId: "test-instance-id",
            zoneName: "us-central1-a");
        var attrs = CreateSampleCloudRunResourceAttributes(platform, gceDetails).ToDictionary(x => x.Key, x => x.Value);
        Assert.NotNull(attrs);
        Assert.Equal(8, attrs.Count);
        Assert.Equal("test-instance-id", attrs[ResourceSemanticConventions.AttributeFaasInstance]);
    }

    [Fact]
    public void TestExtractGaeResourceAttributes()
    {
        var details = new GaePlatformDetails(
            gcloudProject: "gcloudProject",
            gaeInstance: "gaeInstance",
            gaeService: "gaeService",
            gaeVersion: "gaeVersion");
        var platform = new Platform(details);
        var attrs = GcpResourceDetector.ExtractGaeResourceAttributes(platform).ToDictionary(x => x.Key, x => x.Value);
        Assert.NotNull(attrs);
        Assert.Equal(3, attrs.Count);
        Assert.Equal(ResourceAttributeConstants.GcpCloudProviderValue, attrs[ResourceSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("gcloudProject", attrs[ResourceSemanticConventions.AttributeCloudAccount]);
        Assert.Equal(ResourceAttributeConstants.GcpGaePlatformValue, attrs[ResourceSemanticConventions.AttributeCloudPlatform]);
    }

    [Fact]
    public void TestExtractGceResourceAttributes()
    {
        var details = new GcePlatformDetails(
            metadataJson: "json",
            projectId: "projectId",
            instanceId: "instanceId",
            zoneName: "projects/12345/zones/us-central1-a");
        var platform = new Platform(details);
        var attrs = GcpResourceDetector.ExtractGceResourceAttributes(platform).ToDictionary(x => x.Key, x => x.Value);
        Assert.NotNull(attrs);
        Assert.Equal(5, attrs.Count);
        Assert.Equal(ResourceAttributeConstants.GcpCloudProviderValue, attrs[ResourceSemanticConventions.AttributeCloudProvider]);
        Assert.Equal("projectId", attrs[ResourceSemanticConventions.AttributeCloudAccount]);
        Assert.Equal(ResourceAttributeConstants.GcpGcePlatformValue, attrs[ResourceSemanticConventions.AttributeCloudPlatform]);
        Assert.Equal("instanceId", attrs[ResourceSemanticConventions.AttributeHostId]);
    }

    // Test method to extract Cloud Run resource attributes with sample GCE details
    private static List<KeyValuePair<string, object>> CreateSampleCloudRunResourceAttributes(Platform platform, GcePlatformDetails gceDetails)
    {
        var attributeList = new List<KeyValuePair<string, object>>
        {
            new(ResourceSemanticConventions.AttributeCloudProvider, ResourceAttributeConstants.GcpCloudProviderValue),
            new(ResourceSemanticConventions.AttributeCloudAccount, platform.ProjectId),
            new(ResourceSemanticConventions.AttributeCloudAvailabilityZone, platform.CloudRunDetails.Zone),
            new(ResourceSemanticConventions.AttributeCloudPlatform, ResourceAttributeConstants.GcpCloudRunPlatformValue),
            new(ResourceSemanticConventions.AttributeCloudRegion, platform.CloudRunDetails.Region),
            new(ResourceSemanticConventions.AttributeFaasName, platform.CloudRunDetails.ServiceName),
            new(ResourceSemanticConventions.AttributeFaasVersion, platform.CloudRunDetails.RevisionName),
        };

        // For faas.instance, use the GCE instance ID from the metadata service
        if (gceDetails != null && !string.IsNullOrEmpty(gceDetails.InstanceId))
        {
            attributeList.Add(new(ResourceSemanticConventions.AttributeFaasInstance, gceDetails.InstanceId));
        }

        return attributeList;
    }
}
