// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A circular memory stream backed by a fixed-size byte buffer.
    /// </summary>
    internal sealed class CircularMemoryStream : MemoryStream
    {
        private readonly byte[] _buffer;

        internal event EventHandler<ArraySegment<byte>> OnFlush;

        internal CircularMemoryStream(byte[] buffer) : base(buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            _buffer = buffer;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var bytesWritten = 0;

            while (bytesWritten < count)
            {
                var bytesToWrite = Math.Min(_buffer.Length - (int)Position, count - bytesWritten);

                base.Write(buffer, offset, bytesToWrite);

                bytesWritten += bytesToWrite;

                FlushIfFull();
            }
        }

        public override void WriteByte(byte value)
        {
            base.WriteByte(value);

            FlushIfFull();
        }

        public override void Flush()
        {
            base.Flush();

            var handler = OnFlush;

            if (handler != null)
            {
                handler(this, new ArraySegment<byte>(_buffer, offset: 0, count: (int)Position));
            }

            Position = 0;
        }

        private void FlushIfFull()
        {
            if (Position == _buffer.Length)
            {
                Flush();
            }
        }
    }
}
