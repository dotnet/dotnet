// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;

namespace OpenTelemetry.OpAmp.Client.Internal.Utils;

internal sealed class BufferSegment : ReadOnlySequenceSegment<byte>
{
    public BufferSegment(byte[] buffer)
    {
        this.Memory = new ReadOnlyMemory<byte>(buffer);
    }

    public void SetNext(BufferSegment segment)
    {
        this.Next = segment;

        segment.RunningIndex = this.RunningIndex + this.Memory.Length;
    }
}
