// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.InteropServices;
using Xunit;

namespace OpenTelemetry.Resources.OperatingSystem.Test;

public class OperatingSystemDetectorTests
{
    [Fact]
    public void TestOperatingSystemAttributes()
    {
        var resource = ResourceBuilder.CreateEmpty().AddOperatingSystemDetector().Build();
        var resourceAttributes = resource.Attributes.ToDictionary(x => x.Key, x => (string)x.Value);

        string expectedPlatform;
        string expectedDescription;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            expectedPlatform = OperatingSystemSemanticConventions.OperatingSystemsValues.Windows;
            expectedDescription = "Windows";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            expectedPlatform = OperatingSystemSemanticConventions.OperatingSystemsValues.Linux;
            expectedDescription = "Linux";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            expectedPlatform = OperatingSystemSemanticConventions.OperatingSystemsValues.Darwin;
#if NET10_0_OR_GREATER
            expectedDescription = "macOS";
#else
            expectedDescription = "Darwin";
#endif
        }
        else
        {
            throw new PlatformNotSupportedException("Unknown platform");
        }

        Assert.Contains(OperatingSystemSemanticConventions.AttributeOperatingSystemDescription, resourceAttributes.Keys);
        Assert.Contains(OperatingSystemSemanticConventions.AttributeOperatingSystemName, resourceAttributes.Keys);
        Assert.Contains(OperatingSystemSemanticConventions.AttributeOperatingSystemType, resourceAttributes.Keys);
        Assert.Contains(OperatingSystemSemanticConventions.AttributeOperatingSystemVersion, resourceAttributes.Keys);
        Assert.Contains(OperatingSystemSemanticConventions.AttributeOperatingSystemBuildId, resourceAttributes.Keys);

        // Not checking on Linux because the description may vary depending on the distribution.
        if (expectedDescription != "Linux")
        {
            Assert.Contains(expectedDescription, resourceAttributes[OperatingSystemSemanticConventions.AttributeOperatingSystemDescription]);
            Assert.Equal(5, resourceAttributes.Count);
        }

        Assert.Equal(expectedPlatform, resourceAttributes[OperatingSystemSemanticConventions.AttributeOperatingSystemType]);
    }

#if NET
    [Fact]
    public void TestParseMacOSPlist()
    {
        var path = "Samples/SystemVersion.plist";
        var osDetector = new OperatingSystemDetector(
            OperatingSystemSemanticConventions.OperatingSystemsValues.Darwin,
            null,
            null,
            null,
            [path]);
        var attributes = osDetector.Detect().Attributes.ToDictionary(x => x.Key, x => (string)x.Value);

        Assert.Equal("Mac OS X", attributes[OperatingSystemSemanticConventions.AttributeOperatingSystemName]);
        Assert.Equal("10.6.8", attributes[OperatingSystemSemanticConventions.AttributeOperatingSystemVersion]);
        Assert.Equal("10K549", attributes[OperatingSystemSemanticConventions.AttributeOperatingSystemBuildId]);
    }

    [Fact]
    public void TestParseLinuxOsRelease()
    {
        var path = "Samples/os-release";
        var kernelPath = "Samples/kernelOsrelease";
        var osDetector = new OperatingSystemDetector(
            OperatingSystemSemanticConventions.OperatingSystemsValues.Linux,
            null,
            kernelPath,
            [path],
            null);
        var attributes = osDetector.Detect().Attributes.ToDictionary(x => x.Key, x => (string)x.Value);

        Assert.Equal("Ubuntu", attributes[OperatingSystemSemanticConventions.AttributeOperatingSystemName]);
        Assert.Equal("22.04", attributes[OperatingSystemSemanticConventions.AttributeOperatingSystemVersion]);
        Assert.Equal("5.15.0-76-generic", attributes[OperatingSystemSemanticConventions.AttributeOperatingSystemBuildId]);
    }
#endif
}
