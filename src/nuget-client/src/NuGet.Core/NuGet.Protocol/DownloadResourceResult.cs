// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Packaging;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// The result of <see cref="DownloadResource"/>.
    /// </summary>
    public sealed class DownloadResourceResult : IDisposable
    {
        private bool _isDisposed;
        private readonly Stream _stream;
        private readonly PackageReaderBase _packageReader;
        private readonly string _packageSource;

        /// <summary>
        /// Initializes a new <see cref="DownloadResourceResult" /> class.
        /// </summary>
        /// <param name="status">A download resource result status.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="status" />
        /// is either <see cref="DownloadResourceResultStatus.Available" /> or
        /// <see cref="DownloadResourceResultStatus.AvailableWithoutStream" />.</exception>
        public DownloadResourceResult(DownloadResourceResultStatus status)
        {
            if (status == DownloadResourceResultStatus.Available ||
                status == DownloadResourceResultStatus.AvailableWithoutStream)
            {
                throw new ArgumentException("A stream should be provided when the result is available.",
                    nameof(status));
            }

            Status = status;
        }

        /// <summary>
        /// Initializes a new <see cref="DownloadResourceResult" /> class.
        /// </summary>
        /// <param name="stream">A package stream.</param>
        /// <param name="source">A package source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream" /> is <see langword="null" />.</exception>
        public DownloadResourceResult(Stream stream, string source)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            Status = DownloadResourceResultStatus.Available;
            _stream = stream;
            _packageSource = source;
        }

        /// <summary>
        /// Initializes a new <see cref="DownloadResourceResult" /> class.
        /// </summary>
        /// <param name="stream">A package stream.</param>
        /// <param name="packageReader">A package reader.</param>
        /// <param name="source">A package source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream" /> is <see langword="null" />.</exception>
        public DownloadResourceResult(Stream stream, PackageReaderBase packageReader, string source)
            : this(stream, source)
        {
            _packageReader = packageReader;
        }

        /// <summary>
        /// Initializes a new <see cref="DownloadResourceResult" /> class.
        /// </summary>
        /// <param name="packageReader">A package reader.</param>
        /// <param name="source">A package source.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageReader" /> is <see langword="null" />.</exception>
        public DownloadResourceResult(PackageReaderBase packageReader, string source)
        {
            if (packageReader == null)
            {
                throw new ArgumentNullException(nameof(packageReader));
            }

            Status = DownloadResourceResultStatus.AvailableWithoutStream;
            _packageReader = packageReader;
            _packageSource = source;
        }

        public DownloadResourceResultStatus Status { get; }

        public bool SignatureVerified { get; set; }

        /// <summary>
        /// Gets the package <see cref="PackageStream"/>.
        /// </summary>
        /// <remarks>The value may be <see langword="null" />.</remarks>
        public Stream PackageStream => _stream;

        /// <summary>
        /// Gets the source containing this package, if not from cache
        /// </summary>
        /// <remarks>The value may be <see langword="null" />.</remarks>
        public string PackageSource => _packageSource;

        /// <summary>
        /// Gets the <see cref="PackageReaderBase"/> for the package.
        /// </summary>
        /// <remarks>The value may be <see langword="null" />.</remarks>
        public PackageReaderBase PackageReader => _packageReader;

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _stream?.Dispose();
                _packageReader?.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }
    }
}
