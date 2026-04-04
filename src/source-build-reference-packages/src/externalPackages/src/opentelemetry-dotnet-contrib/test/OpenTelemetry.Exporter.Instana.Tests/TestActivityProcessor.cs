// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Exporter.Instana.Implementation;
using OpenTelemetry.Exporter.Instana.Implementation.Processors;

namespace OpenTelemetry.Exporter.Instana.Tests;

internal class TestActivityProcessor : IActivityProcessor
{
    public IActivityProcessor? NextProcessor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Task ProcessAsync(Activity activity, InstanaSpan instanaSpan)
    {
        return Task.CompletedTask;
    }
}
