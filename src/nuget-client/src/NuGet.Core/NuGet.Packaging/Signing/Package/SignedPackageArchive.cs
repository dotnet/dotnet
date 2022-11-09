// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// A nupkg that supports both reading and writing signatures.
    /// </summary>
    public class SignedPackageArchive : PackageArchiveReader, ISignedPackage
    {
        /// <summary>
        /// Stream underlying the ZipArchive. Used to do signature verification on a SignedPackageArchive.
        /// If this is null then we cannot perform signature verification.
        /// </summary>
        private readonly Stream _zipWriteStream;

        public SignedPackageArchive(Stream packageReadStream, Stream packageWriteStream)
            : base(new ZipArchive(packageReadStream, ZipArchiveMode.Read, leaveOpen: true), DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance)
        {
            ZipReadStream = packageReadStream ?? throw new ArgumentNullException(nameof(packageReadStream));
            _zipWriteStream = packageWriteStream ?? throw new ArgumentNullException(nameof(packageWriteStream));
        }

        /// <summary>
        /// Adds a signature to a package if it is not already signed.
        /// </summary>
        /// <param name="signatureStream">Stream of the signature SignedCms object to be added to the package.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns></returns>
        public async Task AddSignatureAsync(Stream signatureStream, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ThrowIfZipReadStreamIsNull();

            if (_zipWriteStream == null)
            {
                throw new SignatureException(Strings.SignedPackageUnableToAccessSignature);
            }

            if (await IsSignedAsync(token))
            {
                throw new SignatureException(NuGetLogCode.NU3001, Strings.SignedPackageAlreadySigned);
            }

            using (var reader = new BinaryReader(ZipReadStream, new UTF8Encoding(), leaveOpen: true))
            using (var writer = new BinaryWriter(
                _zipWriteStream,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true),
                leaveOpen: true))
            {
                SignedPackageArchiveUtility.SignZip((MemoryStream)signatureStream, reader, writer);
            }
        }

        /// <summary>
        /// Remove a signature from the package, if it exists.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        public async Task RemoveSignatureAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (!await IsSignedAsync(token))
            {
                throw new SignatureException(Strings.SignedPackageNotSignedOnRemove);
            }

            using (var reader = new BinaryReader(ZipReadStream, SigningSpecifications.Encoding, leaveOpen: true))
            using (var writer = new BinaryWriter(_zipWriteStream, SigningSpecifications.Encoding, leaveOpen: true))
            {
                SignedPackageArchiveUtility.UnsignZip(reader, writer);
            }
        }

        public Task<bool> IsZip64Async(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ThrowIfZipReadStreamIsNull();

            using (var bufferedStream = new ReadOnlyBufferedStream(ZipReadStream, leaveOpen: true))
            using (var reader = new BinaryReader(bufferedStream, new UTF8Encoding(), leaveOpen: true))
            {
                return Task.FromResult(SignedPackageArchiveUtility.IsZip64(reader));
            }
        }

        internal uint GetPackageEntryCount()
        {
            ThrowIfZipReadStreamIsNull();

            using (var bufferedStream = new ReadOnlyBufferedStream(ZipReadStream, leaveOpen: true))
            using (var reader = new BinaryReader(bufferedStream, new UTF8Encoding(), leaveOpen: true))
            {
                var eocdr = EndOfCentralDirectoryRecord.Read(reader);

                return eocdr.CountOfEntriesInCentralDirectory;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
