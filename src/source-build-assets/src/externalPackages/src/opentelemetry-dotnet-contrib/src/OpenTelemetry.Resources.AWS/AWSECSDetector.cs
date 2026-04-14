// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Text.Json;
using System.Text.RegularExpressions;
using OpenTelemetry.AWS;

namespace OpenTelemetry.Resources.AWS;

/// <summary>
/// Resource detector for application running in AWS ECS.
/// </summary>
internal sealed partial class AWSECSDetector : IResourceDetector
{
    private const string AWSECSMetadataPath = "/proc/self/cgroup";
    private const string AWSECSMetadataURLKey = "ECS_CONTAINER_METADATA_URI";
    private const string AWSECSMetadataURLV4Key = "ECS_CONTAINER_METADATA_URI_V4";

    private readonly AWSSemanticConventions semanticConventionBuilder;

    public AWSECSDetector(AWSSemanticConventions semanticConventionBuilder)
    {
        this.semanticConventionBuilder = semanticConventionBuilder;
    }

    /// <summary>
    /// Detector the required and optional resource attributes from AWS ECS.
    /// </summary>
    /// <returns>Resource with key-value pairs of resource attributes.</returns>
    public Resource Detect()
    {
        if (!IsECSProcess())
        {
            return Resource.Empty;
        }

        var resourceAttributes =
            this.semanticConventionBuilder
                .AttributeBuilder
                .AddAttributeCloudProviderIsAWS()
                .AddAttributeCloudPlatformIsAwsEcs();
        try
        {
            var containerId = GetECSContainerId(AWSECSMetadataPath);
            if (containerId != null)
            {
                resourceAttributes.AddAttributeContainerId(containerId);
            }
        }
        catch (Exception ex)
        {
            AWSResourcesEventSource.Log.ResourceAttributesExtractException(nameof(AWSECSDetector), ex);
        }

        try
        {
            this.ExtractMetadataV4ResourceAttributes(resourceAttributes);
        }
        catch (Exception ex)
        {
            AWSResourcesEventSource.Log.ResourceAttributesExtractException(nameof(AWSECSDetector), ex);
        }

        return new Resource(resourceAttributes.Build());
    }

    internal static string? GetECSContainerId(string path)
    {
        string? containerId = null;

        using (var streamReader = ResourceDetectorUtils.GetStreamReader(path))
        {
            while (!streamReader.EndOfStream)
            {
                var trimmedLine = streamReader.ReadLine()?.Trim();
                if (trimmedLine?.Length > 64)
                {
                    containerId = trimmedLine.Substring(trimmedLine.Length - 64);
                    return containerId;
                }
            }
        }

        return containerId;
    }

    internal static bool IsECSProcess()
    {
        return Environment.GetEnvironmentVariable(AWSECSMetadataURLKey) != null || Environment.GetEnvironmentVariable(AWSECSMetadataURLV4Key) != null;
    }

    internal void ExtractMetadataV4ResourceAttributes(AWSSemanticConventions.AttributeBuilderImpl resourceAttributes)
    {
        var metadataV4Url = Environment.GetEnvironmentVariable(AWSECSMetadataURLV4Key);
        if (metadataV4Url == null)
        {
            return;
        }

        using var scope = SuppressInstrumentationScope.Begin();
        using var httpClientHandler = new HttpClientHandler();
        var metadataV4ContainerResponse = AsyncHelper.RunSync(() => ResourceDetectorUtils.SendOutRequestAsync(metadataV4Url, HttpMethod.Get, null, httpClientHandler));
        var metadataV4TaskResponse = AsyncHelper.RunSync(() => ResourceDetectorUtils.SendOutRequestAsync($"{metadataV4Url.TrimEnd('/')}/task", HttpMethod.Get, null, httpClientHandler));

        using var containerResponse = JsonDocument.Parse(metadataV4ContainerResponse);
        using var taskResponse = JsonDocument.Parse(metadataV4TaskResponse);

        if (!containerResponse.RootElement.TryGetProperty("ContainerARN", out var containerArnElement)
            || containerArnElement.GetString() is not string containerArn)
        {
            AWSResourcesEventSource.Log.ResourceAttributesExtractException(nameof(AWSECSDetector), new ArgumentException("The ECS Metadata V4 response did not contain the 'ContainerARN' field"));
            return;
        }

        if (!taskResponse.RootElement.TryGetProperty("Cluster", out var clusterArnElement)
            || clusterArnElement.GetString() is not string clusterArn)
        {
            AWSResourcesEventSource.Log.ResourceAttributesExtractException(nameof(AWSECSDetector), new ArgumentException("The ECS Metadata V4 response did not contain the 'Cluster' field"));
            return;
        }

        if (!clusterArn.StartsWith("arn:", StringComparison.Ordinal))
        {
            var baseArn = containerArn.Substring(containerArn.LastIndexOf(':'));
        }

        resourceAttributes
            .AddAttributeCloudResourceId(containerArn)
            .AddAttributeEcsContainerArn(containerArn)
            .AddAttributeEcsClusterArn(clusterArn);

        if (taskResponse.RootElement.TryGetProperty("AvailabilityZone", out var availabilityZoneElement) && availabilityZoneElement.ValueKind == JsonValueKind.String)
        {
            resourceAttributes.AddAttributeCloudAvailabilityZone(availabilityZoneElement.GetString()!);
        }

        if (!taskResponse.RootElement.TryGetProperty("LaunchType", out var launchTypeElement))
        {
            launchTypeElement = default;
        }

        if (string.Equals("ec2", launchTypeElement.GetString(), StringComparison.OrdinalIgnoreCase))
        {
            resourceAttributes.AddAttributeEcsLaunchtypeIsEc2();
        }
        else if (string.Equals("fargate", launchTypeElement.GetString(), StringComparison.OrdinalIgnoreCase))
        {
            resourceAttributes.AddAttributeEcsLaunchtypeIsFargate();
        }
        else
        {
            AWSResourcesEventSource.Log.ResourceAttributesExtractException(nameof(AWSECSDetector), new ArgumentException($"The ECS Metadata V4 response contained the unrecognized launch type '{launchTypeElement}'"));
        }

        if (taskResponse.RootElement.TryGetProperty("TaskARN", out var taskArnElement) && taskArnElement.ValueKind == JsonValueKind.String)
        {
            var taskArn = taskArnElement.GetString()!;
            resourceAttributes
                .AddAttributeEcsTaskArn(taskArn);

            var arnParts = taskArn.Split(':');
            if (arnParts.Length > 5)
            {
                resourceAttributes.AddAttributeCloudAccountID(arnParts[4]);
                resourceAttributes.AddAttributeCloudRegion(arnParts[3]);
            }
        }

        if (taskResponse.RootElement.TryGetProperty("Family", out var familyElement) && familyElement.ValueKind == JsonValueKind.String)
        {
            resourceAttributes.AddAttributeEcsTaskFamily(familyElement.GetString()!);
        }

        if (taskResponse.RootElement.TryGetProperty("Revision", out var revisionElement) && revisionElement.ValueKind == JsonValueKind.String)
        {
            resourceAttributes.AddAttributeEcsTaskRevision(revisionElement.GetString()!);
        }

        if (containerResponse.RootElement.TryGetProperty("LogDriver", out var logDriverElement)
            && logDriverElement.ValueKind == JsonValueKind.String
            && logDriverElement.ValueEquals("awslogs"))
        {
            if (containerResponse.RootElement.TryGetProperty("LogOptions", out var logOptionsElement))
            {
                var match = ArnRegex().Match(containerArn);

                if (!match.Success)
                {
                    throw new ArgumentOutOfRangeException($"Cannot parse region and account from the container ARN '{containerArn}'");
                }

                var logsRegion = match.Groups[1];
                var logsAccount = match.Groups[2];

                if (logOptionsElement.TryGetProperty("awslogs-group", out var logGroupElement) && logGroupElement.ValueKind == JsonValueKind.String)
                {
                    var logGroupName = logGroupElement.GetString()!;
                    resourceAttributes.AddAttributeLogGroupNames(new[] { logGroupName });
                    resourceAttributes.AddAttributeLogGroupArns(new[] { $"arn:aws:logs:{logsRegion}:{logsAccount}:log-group:{logGroupName}:*" });

                    if (logOptionsElement.TryGetProperty("awslogs-stream", out var logStreamElement) && logStreamElement.ValueKind == JsonValueKind.String)
                    {
                        var logStreamName = logStreamElement.GetString()!;
                        resourceAttributes.AddAttributeLogStreamNames(new[] { logStreamName });
                        resourceAttributes.AddAttributeLogStreamArns(new[] { $"arn:aws:logs:{logsRegion}:{logsAccount}:log-group:{logGroupName}:log-stream:{logStreamName}" });
                    }
                }
            }
        }
    }

    [GeneratedRegex(@"arn:aws:ecs:([^:]+):([^:]+):.*")]
    private static partial Regex ArnRegex();
}
#endif
