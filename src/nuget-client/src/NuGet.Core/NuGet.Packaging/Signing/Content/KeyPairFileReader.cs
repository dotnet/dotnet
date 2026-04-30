// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NuGet.Packaging.Signing
{
    public class KeyPairFileReader : IDisposable
    {
        private static readonly Regex NamePattern = new Regex("^[a-zA-Z0-9\\.\\-/]+$", RegexOptions.CultureInvariant);

        private readonly StreamReader _reader;
        private bool _disposed;

        public KeyPairFileReader(Stream stream, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            _reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: false);
        }

        /// <summary>
        /// Read a section of key value pairs from the file.
        /// Throw for invalid formats.
        /// </summary>
        /// <remarks>Returns an empty set if the file has reached the end.</remarks>
        public Dictionary<string, string> ReadSection()
        {
            var entries = new Dictionary<string, string>(StringComparer.Ordinal);

            var line = _reader.ReadLine();

            if (line == null)
            {
                return entries;
            }

            while (!string.IsNullOrEmpty(line))
            {
                var property = GetProperty(line);

                if (entries.ContainsKey(property.Key))
                {
                    ThrowInvalidFormat();
                }
                else
                {
                    entries.Add(property.Key, property.Value);
                }

                line = _reader.ReadLine();
            }

            // Read section break.
            if (line != string.Empty)
            {
                ThrowInvalidFormat();
            }

            return entries;
        }

        private static KeyValuePair<string, string> GetProperty(string line)
        {
#if NETCOREAPP
            var pos = line.IndexOf(':', StringComparison.Ordinal);
#else
            var pos = line.IndexOf(':');
#endif

            if (pos > 0)
            {
                var key = line.Substring(0, pos);

                if (NamePattern.IsMatch(key))
                {
                    var value = line.Substring(pos + 1);

                    if (!string.IsNullOrEmpty(value))
                    {
                        return new KeyValuePair<string, string>(key, value);
                    }
                }
            }

            throw new SignatureException(Strings.InvalidSignatureContent);
        }

        private static void ThrowInvalidFormat()
        {
            throw new SignatureException(Strings.InvalidSignatureContent);
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
                _reader.Dispose();
            }

            _disposed = true;
        }
    }
}
