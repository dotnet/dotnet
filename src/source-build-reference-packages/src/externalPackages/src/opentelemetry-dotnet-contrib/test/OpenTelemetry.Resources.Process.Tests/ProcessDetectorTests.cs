// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Resources.Process.Tests;

public class ProcessDetectorTests
{
    [Fact]
    public void TestProcessAttributes()
    {
        var resource = ResourceBuilder.CreateEmpty().AddProcessDetector().Build();

        var resourceAttributes = resource.Attributes.ToDictionary(x => x.Key, x => x.Value);

        Assert.Equal(2, resourceAttributes.Count);

        Assert.IsType<string>(resourceAttributes[ProcessSemanticConventions.AttributeProcessOwner]);
        Assert.IsType<long>(resourceAttributes[ProcessSemanticConventions.AttributeProcessPid]);
    }
}
