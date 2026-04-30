// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public sealed class SigningOptions : IDisposable
    {
        private readonly Lazy<Stream> _inputPackageStream;
        private readonly Lazy<Stream> _outputPackageStream;
        private bool _isDisposed;

        /// <summary>
        /// Readable stream for the package that will be used as an input for any signing operation.
        /// </summary>
        public Stream InputPackageStream
        {
            get
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(nameof(SigningOptions));
                }

                return _inputPackageStream.Value;
            }
        }

        /// <summary>
        /// Readable and writeable stream for the output package for any signing operation.
        /// </summary>
        public Stream OutputPackageStream
        {
            get
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(nameof(SigningOptions));
                }

                return _outputPackageStream.Value;
            }
        }

        /// <summary>
        /// Switch used to indicate if an existing signature should be overwritten.
        /// </summary>
        public bool Overwrite { get; }

        /// <summary>
        /// Provider to create a Signature that can be added to the package.
        /// </summary>
        public ISignatureProvider SignatureProvider { get; }

        /// <summary>
        /// Logger to be used to display the logs during the execution of signing actions.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>Instantiates a new <see cref="SigningOptions" /> object.</summary>
        /// <param name="inputPackageStream">A readable stream for the package that will be used as input for any
        /// signing operation.</param>
        /// <param name="outputPackageStream">A readable and writeable stream for the output package for any signing
        /// operation.</param>
        /// <param name="overwrite">A flag indicating if an existing signature should be overwritten.</param>
        /// <param name="signatureProvider">A provider to create a Signature that can be added to the package.</param>
        /// <param name="logger">A logger.</param>
        /// <remarks>Signing operations cannot be done in place; therefore, <paramref name="inputPackageStream"/>
        /// and <paramref name="outputPackageStream" /> should be different streams.</remarks>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="inputPackageStream" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="outputPackageStream" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signatureProvider" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="inputPackageStream" /> and
        /// <paramref name="outputPackageStream"/> are the same object.</exception>
        public SigningOptions(
            Lazy<Stream> inputPackageStream,
            Lazy<Stream> outputPackageStream,
            bool overwrite,
            ISignatureProvider signatureProvider,
            ILogger logger)
        {
            _inputPackageStream = inputPackageStream ?? throw new ArgumentNullException(nameof(inputPackageStream));
            _outputPackageStream = outputPackageStream ?? throw new ArgumentNullException(nameof(outputPackageStream));
            SignatureProvider = signatureProvider ?? throw new ArgumentNullException(nameof(signatureProvider));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Overwrite = overwrite;
        }

        /// <summary>Creates a new <see cref="SigningOptions" /> object from file paths.</summary>
        /// <param name="inputPackageFilePath">The file path of the package that will be used as input for any
        /// signing operation.</param>
        /// <param name="outputPackageFilePath">The file path of the package that will be the output for any signing
        /// operation.</param>
        /// <param name="overwrite">A flag indicating if an existing signature should be overwritten.</param>
        /// <param name="signatureProvider">A provider to create a Signature that can be added to the package.</param>
        /// <param name="logger">A logger.</param>
        /// <remarks>Signing operations cannot be done in place; therefore, <paramref name="inputPackageFilePath"/>
        /// and <paramref name="outputPackageFilePath" /> should be different file paths.</remarks>
        /// <exception cref="ArgumentException">Thrown if <paramref name="inputPackageFilePath" /> is <see langword="null" />,
        /// an empty string, or equivalent to <paramref name="outputPackageFilePath" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="inputPackageFilePath" /> is <see langword="null" />,
        /// an empty string, or equivalent to <paramref name="outputPackageFilePath" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signatureProvider" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <see langword="null" />.</exception>
        public static SigningOptions CreateFromFilePaths(
            string inputPackageFilePath,
            string outputPackageFilePath,
            bool overwrite,
            ISignatureProvider signatureProvider,
            ILogger logger)
        {
            if (string.IsNullOrEmpty(inputPackageFilePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(inputPackageFilePath));
            }

            if (string.IsNullOrEmpty(outputPackageFilePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(outputPackageFilePath));
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(NormalizeFilePath(inputPackageFilePath), NormalizeFilePath(outputPackageFilePath)))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.SigningCannotBeDoneInPlace,
                        nameof(inputPackageFilePath),
                        nameof(outputPackageFilePath)));
            }

            if (signatureProvider == null)
            {
                throw new ArgumentNullException(nameof(signatureProvider));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            return new SigningOptions(
                new Lazy<Stream>(() => File.OpenRead(inputPackageFilePath)),
                new Lazy<Stream>(() => File.Open(outputPackageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)),
                overwrite,
                signatureProvider,
                logger);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_inputPackageStream.IsValueCreated)
                {
                    _inputPackageStream.Value.Dispose();
                }

                if (_outputPackageStream.IsValueCreated)
                {
                    _outputPackageStream.Value.Dispose();
                }

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        private static string NormalizeFilePath(string filePath)
        {
            return Path.GetFullPath(filePath).TrimEnd('\\');
        }
    }
}
