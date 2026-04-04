// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Exporter.Instana.Implementation;
using Xunit;

namespace OpenTelemetry.Exporter.Instana.Tests;

public static class InstanaSpanFactoryTests
{
    [Fact]
    public static void CreateSpan()
    {
        _ = new InstanaSpanFactory();
        var instanaSpan = InstanaSpanFactory.CreateSpan();

        Assert.NotNull(instanaSpan);
        Assert.NotNull(instanaSpan.TransformInfo);
        Assert.NotNull(instanaSpan.Data);
        Assert.Empty(instanaSpan.Data.data);
        Assert.Empty(instanaSpan.Data.Tags);
        Assert.Empty(instanaSpan.Data.Events);
    }
}
