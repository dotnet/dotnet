// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NuGet.RuntimeModel
{
    /// <summary>
    /// Generates JSON from an object graph.
    ///
    /// This is non-private only to facilitate unit testing.
    /// </summary>
    public sealed class JsonObjectWriter : IObjectWriter, IDisposable
    {
        private readonly JsonWriter _writer;
        private bool _isDisposed;

        public JsonObjectWriter(JsonWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                ((IDisposable)_writer).Dispose();

                _isDisposed = true;
            }
        }

        public void WriteObjectStart()
        {
            ThrowIfDisposed();

            _writer.WriteStartObject();
        }

        public void WriteObjectStart(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();

            _writer.WritePropertyName(name);
            _writer.WriteStartObject();
        }

        public void WriteArrayStart(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();

            _writer.WritePropertyName(name);
            _writer.WriteStartArray();
        }

        public void WriteObjectEnd()
        {
            ThrowIfDisposed();

            _writer.WriteEndObject();
        }

        public void WriteArrayEnd()
        {
            ThrowIfDisposed();

            _writer.WriteEndArray();
        }

        public void WriteNameValue(string name, int value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ThrowIfDisposed();

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

            _writer.WritePropertyName(name);
            _writer.WriteStartArray();

            foreach (string value in values)
            {
                _writer.WriteValue(value);
            }

            _writer.WriteEndArray();
        }

        public void WriteNonEmptyNameArray(string name, IEnumerable<string> values)
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

            // Manually enumerate the IEnumerable so we only write the name
            // when there are corresponding values and avoid potentially expensive
            // multiple enumeration.
            var enumerator = values.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return;
            }

            _writer.WritePropertyName(name);
            _writer.WriteStartArray();
            _writer.WriteValue(enumerator.Current);
            while (enumerator.MoveNext())
            {
                _writer.WriteValue(enumerator.Current);
            }

            _writer.WriteEndArray();
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(JsonObjectWriter));
            }
        }
    }
}
