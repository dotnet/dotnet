// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Resources.ProcessRuntime.Tests;

public class ProcessRuntimeDetectorTests
{
    [Fact]
    public void TestProcessRuntimeAttributes()
    {
        var resource = ResourceBuilder.CreateEmpty().AddProcessRuntimeDetector().Build();

        var resourceAttributes = resource.Attributes.ToDictionary(x => x.Key, x => x.Value);

        Assert.Equal(3, resourceAttributes.Count);

#if NETFRAMEWORK
        var strResult = Assert.IsType<string>(resourceAttributes[ProcessRuntimeSemanticConventions.AttributeProcessRuntimeDescription]);
        Assert.Matches(@"^\.NET Framework \d+\.\d+\.\d+\.\d+$", strResult);
        strResult = Assert.IsType<string>(resourceAttributes[ProcessRuntimeSemanticConventions.AttributeProcessRuntimeName]);
        Assert.Equal(".NET Framework", strResult);
        strResult = Assert.IsType<string>(resourceAttributes[ProcessRuntimeSemanticConventions.AttributeProcessRuntimeVersion]);
        Assert.Matches(@"^4.[5-9](.[1-2])?$", strResult);
#else
        var strResult = Assert.IsType<string>(resourceAttributes[ProcessRuntimeSemanticConventions.AttributeProcessRuntimeDescription]);
        Assert.Matches(@"^\.NET \d+\.\d+\.\d+(\-(preview|rc)\.\d+\.\d+\.\d+)?$", strResult);
        strResult = Assert.IsType<string>(resourceAttributes[ProcessRuntimeSemanticConventions.AttributeProcessRuntimeName]);
        Assert.Equal(".NET", strResult);
        strResult = Assert.IsType<string>(resourceAttributes[ProcessRuntimeSemanticConventions.AttributeProcessRuntimeVersion]);
        Assert.Matches(@"^(([1-3,5-9])|(\d{2,}))\.\d+\.\d+$", strResult);
#endif
    }
}
