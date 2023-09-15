// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;

namespace NuGet.Packaging
{
    /// <summary>
    /// Reads a development nupkg
    /// </summary>
    public class PackageArchiveReader : PackageReaderBase
    {
        private readonly ZipArchive _zipArchive;
        private readonly SigningSpecifications _signingSpecifications = SigningSpecifications.V1;
        private readonly IEnvironmentVariableReader _environmentVariableReader;

        /// <summary>
        /// Signature specifications.
        /// </summary>
        protected SigningSpecifications SigningSpecifications => _signingSpecifications;

        /// <summary>
        /// Stream underlying the ZipArchive. Used to do signature verification on a SignedPackageArchive.
        /// If this is null then we cannot perform signature verification.
        /// </summary>
        protected Stream ZipReadStream { get; set; }

#if IS_SIGNING_SUPPORTED
        /// <summary>
        /// True if the package is signed
        /// </summary>
        private bool? _isSigned;
#endif

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="frameworkProvider">Framework mapping provider for NuGetFramework parsing.</param>
        /// <param name="compatibilityProvider">Framework compatibility provider.</param>
        private PackageArchiveReader(IFrameworkNameProvider frameworkProvider, IFrameworkCompatibilityProvider compatibilityProvider)
            : base(frameworkProvider, compatibilityProvider)
        {
            _environmentVariableReader = EnvironmentVariableWrapper.Instance;
        }

        // For testing purposes only
        internal PackageArchiveReader(Stream stream, IEnvironmentVariableReader environmentVariableReader)
            : this(stream)
        {
            if (environmentVariableReader != null)
            {
                _environmentVariableReader = environmentVariableReader;
            }
        }

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="stream">Nupkg data stream.</param>
        public PackageArchiveReader(Stream stream)
            : this(stream, false, DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance)
        {
        }

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="stream">Nupkg data stream.</param>
        /// <param name="frameworkProvider">Framework mapping provider for NuGetFramework parsing.</param>
        /// <param name="compatibilityProvider">Framework compatibility provider.</param>
        public PackageArchiveReader(Stream stream, IFrameworkNameProvider frameworkProvider, IFrameworkCompatibilityProvider compatibilityProvider)
            : this(stream, false)
        {
        }

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="stream">Nupkg data stream.</param>
        /// <param name="leaveStreamOpen">If true the nupkg stream will not be closed by the zip reader.</param>
        public PackageArchiveReader(Stream stream, bool leaveStreamOpen)
            : this(new ZipArchive(stream, ZipArchiveMode.Read, leaveStreamOpen), DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance)
        {
            ZipReadStream = stream;
        }

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="stream">Nupkg data stream.</param>
        /// <param name="leaveStreamOpen">leave nupkg stream open</param>
        /// <param name="frameworkProvider">Framework mapping provider for NuGetFramework parsing.</param>
        /// <param name="compatibilityProvider">Framework compatibility provider.</param>
        public PackageArchiveReader(Stream stream, bool leaveStreamOpen, IFrameworkNameProvider frameworkProvider, IFrameworkCompatibilityProvider compatibilityProvider)
            : this(new ZipArchive(stream, ZipArchiveMode.Read, leaveStreamOpen), frameworkProvider, compatibilityProvider)
        {
            ZipReadStream = stream;
        }

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="zipArchive">ZipArchive containing the nupkg data.</param>
        public PackageArchiveReader(ZipArchive zipArchive)
            : this(zipArchive, DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance)
        {
        }

        /// <summary>
        /// Nupkg package reader
        /// </summary>
        /// <param name="zipArchive">ZipArchive containing the nupkg data.</param>
        /// <param name="frameworkProvider">Framework mapping provider for NuGetFramework parsing.</param>
        /// <param name="compatibilityProvider">Framework compatibility provider.</param>
        public PackageArchiveReader(ZipArchive zipArchive, IFrameworkNameProvider frameworkProvider, IFrameworkCompatibilityProvider compatibilityProvider)
            : this(frameworkProvider, compatibilityProvider)
        {
            _zipArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));
        }

        public PackageArchiveReader(string filePath, IFrameworkNameProvider frameworkProvider = null, IFrameworkCompatibilityProvider compatibilityProvider = null)
            : this(frameworkProvider ?? DefaultFrameworkNameProvider.Instance, compatibilityProvider ?? DefaultCompatibilityProvider.Instance)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            // Since this constructor owns the stream, the responsibility falls here to dispose the stream of an
            // invalid .zip archive. If this constructor succeeds, the disposal of the stream is handled by the
            // disposal of this instance.
            Stream stream = null;
            try
            {
                stream = File.OpenRead(filePath);
                ZipReadStream = stream;
                _zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
            }
            catch (Exception ex)
            {
                stream?.Dispose();
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPackageNupkg, filePath), ex);
            }
        }

        public override IEnumerable<string> GetFiles()
        {
            return _zipArchive.GetFiles();
        }

        public override IEnumerable<string> GetFiles(string folder)
        {
            return GetFiles().Where(f => f.StartsWith(folder + "/", StringComparison.OrdinalIgnoreCase));
        }

        public override Stream GetStream(string path)
        {
            Stream stream = null;
            path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!string.IsNullOrEmpty(path))
            {
                stream = _zipArchive.OpenFile(path);
            }

            return stream;
        }

        /// <summary>
        /// Asynchronously copies a package to the specified destination file path.
        /// </summary>
        /// <param name="nupkgFilePath">The destination file path.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="string" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="nupkgFilePath" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public override async Task<string> CopyNupkgAsync(
            string nupkgFilePath,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(nupkgFilePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(nupkgFilePath));
            }

            cancellationToken.ThrowIfCancellationRequested();

            ZipReadStream.Seek(offset: 0, origin: SeekOrigin.Begin);

            using (var destination = File.OpenWrite(nupkgFilePath))
            {
#if NETCOREAPP2_0_OR_GREATER
                await ZipReadStream.CopyToAsync(destination, cancellationToken);
#else
                const int BufferSize = 8192;
                await ZipReadStream.CopyToAsync(destination, BufferSize, cancellationToken);
#endif
            }

            return nupkgFilePath;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _zipArchive.Dispose();
            }
        }

        /// <summary>
        /// This class literally just exists so CopyToFile gets a file size
        /// </summary>
        private sealed class SizedArchiveEntryStream : Stream
        {
            private readonly Stream _inner;

            private readonly long _size;

            private bool _isDisposed;

            public SizedArchiveEntryStream(Stream inner, long size)
            {
                _inner = inner;
                _size = size;
            }

            public override long Length { get => _size; }

            public override bool CanRead => _inner.CanRead;

            public override bool CanSeek => _inner.CanSeek;

            public override bool CanWrite => _inner.CanWrite;

            public override long Position { get => _inner.Position; set => _inner.Position = value; }

            public override void Flush()
            {
                _inner.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _inner.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _inner.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _inner.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _inner.Write(buffer, offset, count);
            }

            protected override void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                    {
                        _inner.Dispose();
                    }

                    _isDisposed = true;
                }
            }
        }

        public override IEnumerable<string> CopyFiles(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken token)
        {
            var filesCopied = new List<string>();
            var packageIdentity = GetIdentity();

            foreach (var packageFile in packageFiles)
            {
                token.ThrowIfCancellationRequested();

                var entry = GetEntry(packageFile);

                var packageFileName = entry.FullName;
                // An entry in a ZipArchive could start with a '/' based on how it is zipped
                // Remove it if present
                if (packageFileName.StartsWith("/", StringComparison.Ordinal))
                {
                    packageFileName = packageFileName.Substring(1);
                }

                // ZipArchive always has forward slashes in them. By replacing them with DirectorySeparatorChar;
                // in windows, we get the windows-style path
                var normalizedPath = Uri.UnescapeDataString(packageFileName.Replace('/', Path.DirectorySeparatorChar));

                destination = NormalizeDirectoryPath(destination);
                ValidatePackageEntry(destination, normalizedPath, packageIdentity);

                var targetFilePath = Path.Combine(destination, normalizedPath);

                using (var stream = entry.Open())
                using (var sizedStream = new SizedArchiveEntryStream(stream, entry.Length))
                {
                    string copiedFile = extractFile(packageFileName, targetFilePath, sizedStream);
                    if (copiedFile != null)
                    {
                        entry.UpdateFileTimeFromEntry(copiedFile, logger);

                        filesCopied.Add(copiedFile);
                    }
                }
            }

            return filesCopied;
        }

        public string ExtractFile(string packageFile, string targetFilePath, ILogger logger)
        {
            var entry = GetEntry(packageFile);
            var copiedFile = entry.SaveAsFile(targetFilePath, logger);
            return copiedFile;
        }

        public ZipArchiveEntry GetEntry(string packageFile)
        {
            return _zipArchive.LookupEntry(packageFile);
        }

        public IEnumerable<ZipFilePair> EnumeratePackageEntries(IEnumerable<string> packageFiles, string packageDirectory)
        {
            foreach (var packageFile in packageFiles)
            {
                var packageFileFullPath = Path.Combine(packageDirectory, packageFile);
                var entry = GetEntry(packageFile);
                yield return new ZipFilePair(packageFileFullPath, entry);
            }
        }

        /// <summary>
        /// Validate all files in package are not traversed outside of the expected extraction path.
        /// Eg: file entry like ../../foo.dll can get outside of the expected extraction path.
        /// </summary>
        public async Task ValidatePackageEntriesAsync(CancellationToken token)
        {
            try
            {
                var files = await GetFilesAsync(token);
                var packageIdentity = await GetIdentityAsync(token);

                // This dummy destination is used to check if the file in package get outside of the extractionPath
                var dummyDestination = NuGetEnvironment.GetFolderPath(NuGetFolderPath.NuGetHome);

                dummyDestination = NormalizeDirectoryPath(dummyDestination);

                ValidatePackageEntries(dummyDestination, files, packageIdentity);
            }
            catch (UnsafePackageEntryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PackagingException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorUnableCheckPackageEntries), e);
            }
        }

        public override async Task<PrimarySignature> GetPrimarySignatureAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ThrowIfZipReadStreamIsNull();

            PrimarySignature signature = null;

            if (await IsSignedAsync(token))
            {
                using (var bufferedStream = new ReadOnlyBufferedStream(ZipReadStream, leaveOpen: true))
                using (var reader = new BinaryReader(bufferedStream, new UTF8Encoding(), leaveOpen: true))
                using (var stream = SignedPackageArchiveUtility.OpenPackageSignatureFileStream(reader))
                {
#if IS_SIGNING_SUPPORTED
                    signature = PrimarySignature.Load(stream);
#endif
                }
            }

            return signature;
        }

        public override Task<bool> IsSignedAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ThrowIfZipReadStreamIsNull();

#if IS_SIGNING_SUPPORTED
            if (!_isSigned.HasValue)
            {
                _isSigned = false;

                using (var zip = new ZipArchive(ZipReadStream, ZipArchiveMode.Read, leaveOpen: true))
                {
                    var signatureEntry = zip.GetEntry(SigningSpecifications.SignaturePath);

                    if (signatureEntry != null &&
                       string.Equals(signatureEntry.Name, SigningSpecifications.SignaturePath, StringComparison.Ordinal))
                    {
                        _isSigned = true;
                    }
                }
            }

            return Task.FromResult(_isSigned.Value);
#else
            return TaskResult.False;
#endif
        }

        public override async Task ValidateIntegrityAsync(SignatureContent signatureContent, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (signatureContent == null)
            {
                throw new ArgumentNullException(nameof(signatureContent));
            }

            ThrowIfZipReadStreamIsNull();

            if (!await IsSignedAsync(token))
            {
                throw new SignatureException(Strings.SignedPackageNotSignedOnVerify);
            }

#if IS_SIGNING_SUPPORTED
            using (var bufferedStream = new ReadOnlyBufferedStream(ZipReadStream, leaveOpen: true))
            using (var reader = new BinaryReader(bufferedStream, new UTF8Encoding(), leaveOpen: true))
            using (var hashAlgorithm = signatureContent.HashAlgorithm.GetHashProvider())
            {
                var expectedHash = Convert.FromBase64String(signatureContent.HashValue);

                if (!SignedPackageArchiveUtility.VerifySignedPackageIntegrity(reader, hashAlgorithm, expectedHash))
                {
                    throw new SignatureException(NuGetLogCode.NU3008, Strings.SignaturePackageIntegrityFailure, GetIdentity());
                }
            }
#endif
        }

        public override string GetContentHash(CancellationToken token, Func<string> GetUnsignedPackageHash = null)
        {
            // Try to get the content hash for signed packages
            var contentHash = GetContentHashForSignedPackage(token);

            if (string.IsNullOrEmpty(contentHash))
            {
                // The package is unsigned, try to read the existing sha512 file
                if (GetUnsignedPackageHash != null)
                {
                    var packageHash = GetUnsignedPackageHash();

                    if (!string.IsNullOrEmpty(packageHash))
                    {
                        return packageHash;
                    }
                }

                ThrowIfZipReadStreamIsNull();

                ZipReadStream.Seek(offset: 0, origin: SeekOrigin.Begin);

                contentHash = Convert.ToBase64String(new CryptoHashProvider("SHA512").CalculateHash(ZipReadStream));
            }

            return contentHash;
        }

        public override Task<byte[]> GetArchiveHashAsync(HashAlgorithmName hashAlgorithmName, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ThrowIfZipReadStreamIsNull();

            ZipReadStream.Seek(offset: 0, origin: SeekOrigin.Begin);

            using (var hashAlgorithm = hashAlgorithmName.GetHashProvider())
            {
                var hash = hashAlgorithm.ComputeHash(ZipReadStream, leaveStreamOpen: true);

                return Task.FromResult(hash);
            }
        }

        public override bool CanVerifySignedPackages(SignedPackageVerifierSettings verifierSettings)
        {
#if IS_SIGNING_SUPPORTED
            // Mono support has been deprioritized, so verification on Mono is not enabled, tracking issue: https://github.com/NuGet/Home/issues/9027
            if (RuntimeEnvironmentHelper.IsMono)
            {
                return false;
            }
            else if (RuntimeEnvironmentHelper.IsLinux || RuntimeEnvironmentHelper.IsMacOSX)
            {
                // Please note: Linux/MAC case sensitive for env var name.
                string signVerifyEnvVariable = _environmentVariableReader.GetEnvironmentVariable(
                    EnvironmentVariableConstants.DotNetNuGetSignatureVerification);

                bool canVerify = false;

                if (!string.IsNullOrEmpty(signVerifyEnvVariable))
                {
                    if (string.Equals(bool.TrueString, signVerifyEnvVariable, StringComparison.OrdinalIgnoreCase))
                    {
                        canVerify = true;
                    }
                    else if (string.Equals(bool.FalseString, signVerifyEnvVariable, StringComparison.OrdinalIgnoreCase))
                    {
                        canVerify = false;
                    }
                }

                return canVerify;
            }
            else
            {
                return true;
            }

#else
            return false;
#endif
        }

        protected void ThrowIfZipReadStreamIsNull()
        {
            if (ZipReadStream == null)
            {
                throw new SignatureException(Strings.SignedPackageUnableToAccessSignature);
            }
        }

        private string GetContentHashForSignedPackage(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (ZipReadStream == null)
            {
                return null;
            }

            using (var zip = new ZipArchive(ZipReadStream, ZipArchiveMode.Read, leaveOpen: true))
            {
                var signatureEntry = zip.GetEntry(SigningSpecifications.SignaturePath);

                if (signatureEntry == null ||
                    !string.Equals(signatureEntry.Name, SigningSpecifications.SignaturePath, StringComparison.Ordinal))
                {
                    return null;
                }
            }

            using (var bufferedStream = new ReadOnlyBufferedStream(ZipReadStream, leaveOpen: true))
            using (var reader = new BinaryReader(bufferedStream, new UTF8Encoding(), leaveOpen: true))
            {
                return SignedPackageArchiveUtility.GetPackageContentHash(reader);
            }
        }
    }
}
