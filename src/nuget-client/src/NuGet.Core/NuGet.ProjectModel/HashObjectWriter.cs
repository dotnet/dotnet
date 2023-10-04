// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.RuntimeModel;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Generates a hash from an object graph.
    ///
    /// This is non-private only to facilitate unit testing.
    /// </summary>
    public sealed class HashObjectWriter : IObjectWriter, IDisposable
    {
        private const int DefaultBufferSize = 4096;

        private readonly byte[] _buffer;
        private readonly IHashFunction _hashFunc;
        private bool _isDisposed;
        private bool _isReadOnly;
        private int _nestLevel;
        private readonly CircularMemoryStream _stream;
        private readonly StreamWriter _streamWriter;
        private readonly JsonTextWriter _writer;

        /// <summary>
        /// Creates a new instance with the provide hash function.
        /// </summary>
        /// <param name="hashFunc">An <see cref="IHashFunction"/> instance.  Throws if <see langword="null" />.</param>
        public HashObjectWriter(IHashFunction hashFunc)
        {
            if (hashFunc == null)
            {
                throw new ArgumentNullException(nameof(hashFunc));
            }

            _buffer = new byte[DefaultBufferSize];
            _hashFunc = hashFunc;
            _stream = new CircularMemoryStream(_buffer);
            _streamWriter = new StreamWriter(_stream);
            _writer = new JsonTextWriter(_streamWriter);

            _stream.OnFlush += OnFlush;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _stream.OnFlush -= OnFlush;

                _hashFunc.Dispose();
                _writer.Close();
                _streamWriter.Dispose();
                _stream.Dispose();

                _isDisposed = true;
            }
        }

        public void WriteObjectStart()
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WriteStartObject();

            ++_nestLevel;
        }

        public void WriteObjectStart(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WritePropertyName(name);
            _writer.WriteStartObject();

            ++_nestLevel;
        }

        public void WriteObjectEnd()
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            if (_nestLevel == 0)
            {
                throw new InvalidOperationException();
            }

            _writer.WriteEndObject();

            --_nestLevel;
        }

        public void WriteNameValue(string name, int value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WritePropertyName(name);
            _writer.WriteValue(value);
        }

        public void WriteNameValue(string name, bool value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WritePropertyName(name);
            _writer.WriteValue(value);
        }

        public void WriteNameValue(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WritePropertyName(name);
            _writer.WriteValue(value);
        }

        public void WriteNameArray(string name, IEnumerable<string> values)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WritePropertyName(name);
            _writer.WriteStartArray();

            foreach (string value in values)
            {
                _writer.WriteValue(value);
            }

            _writer.WriteEndArray();
        }

        /// <summary>
        /// Gets the hash for the object.
        ///
        /// Once GetHash is called, no further writing is allowed.
        /// </summary>
        /// <returns>A hash of the object.</returns>
        public string GetHash()
        {
            ThrowIfDisposed();

            if (!_isReadOnly)
            {
                _writer.Flush();

                _isReadOnly = true;
            }

            return _hashFunc.GetHash();
        }

        public void WriteArrayStart(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _writer.WritePropertyName(name);
            _writer.WriteStartArray();

            ++_nestLevel;
        }

        public void WriteArrayEnd()
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            if (_nestLevel == 0)
            {
                throw new InvalidOperationException();
            }

            _writer.WriteEndArray();

            --_nestLevel;
        }

        private void OnFlush(object sender, ArraySegment<byte> bytes)
        {
            if (bytes.Count > 0)
            {
                _hashFunc.Update(bytes.Array, bytes.Offset, bytes.Count);
            }
        }

        private void ThrowIfReadOnly()
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(HashObjectWriter));
            }
        }
    }
}
