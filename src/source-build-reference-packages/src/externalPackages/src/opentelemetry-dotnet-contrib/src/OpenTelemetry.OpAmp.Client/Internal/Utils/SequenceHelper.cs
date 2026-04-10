// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;

namespace OpenTelemetry.OpAmp.Client.Internal.Utils;

internal static class SequenceHelper
{
    public static ReadOnlySequence<byte> AsSequence(this byte[] message)
    {
        if (message == null || message.Length == 0)
        {
            return ReadOnlySequence<byte>.Empty;
        }

        return new ReadOnlySequence<byte>(message);
    }

    public static ReadOnlySequence<byte> CreateSequenceFromBuffers(this IReadOnlyList<byte[]> buffers, int endIndex)
    {
        if (buffers == null || buffers.Count == 0)
        {
            return ReadOnlySequence<byte>.Empty;
        }

        if (buffers.Count == 1)
        {
            return new ReadOnlySequence<byte>(buffers[0], 0, endIndex);
        }

        BufferSegment segment1 = new BufferSegment(buffers[0]);
        BufferSegment currentSegment = segment1;

        for (int i = 1; i < buffers.Count; i++)
        {
            BufferSegment nextSegment = new BufferSegment(buffers[i]);

            currentSegment.SetNext(nextSegment);
            currentSegment = nextSegment;
        }

        return new ReadOnlySequence<byte>(segment1, 0, currentSegment, endIndex);
    }
}
