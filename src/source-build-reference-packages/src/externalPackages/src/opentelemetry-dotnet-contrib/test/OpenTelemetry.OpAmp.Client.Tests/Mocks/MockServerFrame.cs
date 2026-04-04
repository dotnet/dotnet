// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Tests.Mocks;

internal class MockServerFrame
{
    public ArraySegment<byte> Frame { get; set; }

    public bool HasHeader { get; set; }

    public string? ExptectedContent { get; set; }

    public int Size { get; internal set; }
}
