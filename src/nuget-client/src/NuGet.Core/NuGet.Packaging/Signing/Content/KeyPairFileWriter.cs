// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;

namespace NuGet.Packaging.Signing
{
    public class KeyPairFileWriter : IDisposable
    {
        private readonly StreamWriter _writer;
        private bool _disposed;

        public KeyPairFileWriter(Stream stream, Encoding encoding, bool leaveOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            _writer = new StreamWriter(stream, encoding, bufferSize: 8192, leaveOpen: leaveOpen);
        }

        /// <summary>
        /// Write key:value with EOL to the manifest stream.
        /// </summary>
        public void WritePair(string key, string value)
        {
            _writer.Write(FormatItem(key, value));
            WriteEOL();
        }

        /// <summary>
        /// Write an empty line.
        /// </summary>
        public void WriteSectionBreak()
        {
            WriteEOL();
        }

        /// <summary>
        /// Write an end of line to the manifest writer.
        /// </summary>
        private void WriteEOL()
        {
            _writer.Write('\n');
        }

        /// <summary>
        /// key:value
        /// </summary>
        private static string FormatItem(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(null, nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(null, nameof(value));
            }

            return $"{key}:{value}";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _writer.Dispose();
            }

            _disposed = true;
        }
    }
}
