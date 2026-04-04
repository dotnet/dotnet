// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Exporter.Instana.Implementation;

namespace OpenTelemetry.Exporter.Instana.Tests;

internal class TestSpanSender : ISpanSender
{
    public Action<InstanaSpan>? OnEnqueue { get; set; }

    public void Enqueue(InstanaSpan instanaSpan)
    {
        this.OnEnqueue?.Invoke(instanaSpan);
    }
}
