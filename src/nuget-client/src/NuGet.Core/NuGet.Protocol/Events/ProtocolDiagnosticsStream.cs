// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Events
{
    internal sealed class ProtocolDiagnosticsStream : Stream
    {
        private readonly Stream _baseStream;
        private ProtocolDiagnosticInProgressHttpEvent _inProgressEvent;
        private readonly Stopwatch _stopwatch;
        private long _bytes;
        private readonly Action<ProtocolDiagnosticHttpEvent> _diagnosticEvent;

        internal ProtocolDiagnosticsStream(Stream baseStream, ProtocolDiagnosticInProgressHttpEvent inProgressEvent, Stopwatch stopwatch, Action<ProtocolDiagnosticHttpEvent> diagnosticEvent)
        {
            _baseStream = baseStream;
            _inProgressEvent = inProgressEvent;
            _stopwatch = stopwatch;
            _bytes = 0;
            _diagnosticEvent = diagnosticEvent;
        }

        public override bool CanRead => _baseStream.CanRead;

        public override bool CanSeek => _baseStream.CanSeek;

        public override bool CanWrite => _baseStream.CanWrite;

        public override long Length => _baseStream.Length;

        public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                var read = _baseStream.Read(buffer, offset, count);
                if (read > 0)
                {
                    _bytes += read;
                }
                else
                {
                    RaiseDiagnosticEvent(isSuccess: true);
                }
                return read;
            }
            catch
            {
                RaiseDiagnosticEvent(isSuccess: false);
                throw;
            }
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            try
            {
                var read = await _baseStream.ReadAsync(buffer, offset, count, cancellationToken);
                if (read > 0)
                {
                    _bytes += read;
                }
                else
                {
                    RaiseDiagnosticEvent(isSuccess: true);
                }
                return read;
            }
            catch
            {
                RaiseDiagnosticEvent(isSuccess: false);
                throw;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            RaiseDiagnosticEvent(isSuccess: true);

            base.Dispose(disposing);

            _baseStream?.Dispose();
        }

        private void RaiseDiagnosticEvent(bool isSuccess)
        {
            if (_inProgressEvent != null)
            {
                var pde = new ProtocolDiagnosticHttpEvent(
                    DateTime.UtcNow,
                    _stopwatch.Elapsed,
                    _bytes,
                    isSuccess,
                    _inProgressEvent);
                _diagnosticEvent(pde);

                _inProgressEvent = null;
            }
        }
    }
}
