// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public class DownloadTimeoutStream : Stream
    {
        private readonly string _downloadName;
        private readonly Stream _networkStream;
        private readonly TimeSpan _timeout;

        public DownloadTimeoutStream(string downloadName, Stream networkStream, TimeSpan timeout)
        {
            if (downloadName == null)
            {
                throw new ArgumentNullException(nameof(downloadName));
            }

            if (networkStream == null)
            {
                throw new ArgumentNullException(nameof(networkStream));
            }

            _downloadName = downloadName;
            _networkStream = networkStream;
            _timeout = timeout;
            if (networkStream.CanTimeout)
            {
                networkStream.ReadTimeout = (int)timeout.TotalMilliseconds;
            }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _networkStream.Read(buffer, offset, count);
        }

#if !IS_CORECLR
        public override IAsyncResult BeginRead(
            byte[] buffer,
            int offset,
            int count,
            AsyncCallback callback,
            object state)
        {
            throw new NotSupportedException();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }
#endif

        public override async Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            var timeoutMessage = string.Format(
                CultureInfo.CurrentCulture,
                Strings.Error_DownloadTimeout,
                _downloadName,
                _timeout.TotalMilliseconds);

            try
            {
                var result = await TimeoutUtility.StartWithTimeout(
                    getTask: timeoutToken => _networkStream.ReadAsync(buffer, offset, count, timeoutToken),
                    timeout: _timeout,
                    timeoutMessage: null,
                    token: cancellationToken).ConfigureAwait(false);

                return result;
            }
            catch (TimeoutException e)
            {
                // Failed stream operations should throw an IOException.
                throw new IOException(timeoutMessage, e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _networkStream.Dispose();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead { get; } = true;

        public override bool CanSeek { get; } = false;

        public override bool CanWrite { get; } = false;

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
    }
}
