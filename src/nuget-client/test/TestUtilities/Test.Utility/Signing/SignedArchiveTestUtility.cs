// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;
#endif
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace Test.Utility.Signing
{
    public static class SignedArchiveTestUtility
    {
#if IS_SIGNING_SUPPORTED
        /// <summary>
        /// Generates an author signed copy of a package and returns the path to that package
        /// This method can timestamp a package and should only be used with tests marked with [CIOnlyFact]
        /// </summary>
        /// <param name="certificate">Certificate to be used while signing the package</param>
        /// <param name="nupkg">Package to be signed</param>
        /// <param name="dir">Directory for placing the signed package</param>
        /// <param name="timestampService">RFC 3161 timestamp service URL.</param>
        /// <param name="signatureHashAlgorithm">Hash algorithm to be used in the signature. Defaults to SHA256</param>
        /// <param name="timestampHashAlgorithm">Hash algorithm to be used in the timestamp. Defaults to SHA256</param>
        /// <returns>Path to the signed copy of the package</returns>
        public static async Task<string> AuthorSignPackageAsync(
            X509Certificate2 certificate,
            SimpleTestPackageContext nupkg,
            string dir,
            Uri timestampService = null,
            HashAlgorithmName signatureHashAlgorithm = HashAlgorithmName.SHA256,
            HashAlgorithmName timestampHashAlgorithm = HashAlgorithmName.SHA256)
        {
            var signedPackagePath = Path.Combine(dir, $"{nupkg.Id}.{nupkg.Version}.nupkg");
            var tempPath = Path.GetTempFileName();

            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var fileStream = File.OpenWrite(tempPath))
            {
                packageStream.CopyTo(fileStream);
            }

            return await AuthorSignPackageAsync(certificate, timestampService, signatureHashAlgorithm, timestampHashAlgorithm, signedPackagePath, tempPath);
        }
#endif
#if IS_SIGNING_SUPPORTED
        /// <summary>
        /// Generates an author signed copy of a package and returns the path to that package
        /// This method can timestamp a package and should only be used with tests marked with [CIOnlyFact]
        /// </summary>
        /// <param name="certificate">Certificate to be used while signing the package</param>
        /// <param name="packageStream">Stream of package to be signed</param>
        /// <param name="packageId">The package ID.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <param name="destinationDirectoryPath">Directory for placing the signed package</param>
        /// <param name="timestampService">RFC 3161 timestamp service URL.</param>
        /// <param name="signatureHashAlgorithm">Hash algorithm to be used in the signature. Defaults to SHA256</param>
        /// <param name="timestampHashAlgorithm">Hash algorithm to be used in the timestamp. Defaults to SHA256</param>
        /// <returns>Path to the signed copy of the package</returns>
        public static async Task<string> AuthorSignPackageAsync(
            X509Certificate2 certificate,
            MemoryStream packageStream,
            string packageId,
            string packageVersion,
            string destinationDirectoryPath,
            Uri timestampService = null,
            HashAlgorithmName signatureHashAlgorithm = HashAlgorithmName.SHA256,
            HashAlgorithmName timestampHashAlgorithm = HashAlgorithmName.SHA256)
        {
            string signedPackagePath = Path.Combine(destinationDirectoryPath, $"{packageId}.{packageVersion}.nupkg");
            string tempPath = Path.GetTempFileName();

            packageStream.Seek(offset: 0, loc: SeekOrigin.Begin);

            using (FileStream fileStream = File.OpenWrite(tempPath))
            {
                packageStream.CopyTo(fileStream);
            }

            return await AuthorSignPackageAsync(certificate, timestampService, signatureHashAlgorithm, timestampHashAlgorithm, signedPackagePath, tempPath);
        }
#endif
#if IS_SIGNING_SUPPORTED
        private static async Task<string> AuthorSignPackageAsync(
            X509Certificate2 certificate,
            Uri timestampService,
            HashAlgorithmName signatureHashAlgorithm,
            HashAlgorithmName timestampHashAlgorithm,
            string signedPackagePath,
            string originalPackagePath)
        {
            using (FileStream originalPackage = File.OpenRead(originalPackagePath))
            using (FileStream signedPackage = File.Open(signedPackagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var request = new AuthorSignPackageRequest(
                new X509Certificate2(certificate),
                signatureHashAlgorithm,
                timestampHashAlgorithm))
            {
                await CreateSignedPackageAsync(request, originalPackage, signedPackage, timestampService);
            }

            FileUtility.Delete(originalPackagePath);

            return signedPackagePath;
        }

        /// <summary>
        /// Generates an repository signed copy of a package and returns the path to that package
        /// This method can timestamp a package and should only be used with tests marked with [CIOnlyFact]
        /// </summary>
        /// <param name="certificate">Certificate to be used while signing the package</param>
        /// <param name="nupkg">Package to be signed</param>
        /// <param name="dir">Directory for placing the signed package</param>
        /// <param name="timestampService">RFC 3161 timestamp service URL.</param>
        /// <param name="v3ServiceIndex">Value for the V3ServiceIndex for the repository signature attribute</param>
        /// <param name="packageOwners">List of package owners for teh repository signature attribute</param>
        /// <param name="signatureHashAlgorithm">Hash algorithm to be used in the signature. Defaults to SHA256</param>
        /// <param name="timestampHashAlgorithm">Hash algorithm to be used in the timestamp. Defaults to SHA256</param>
        /// <returns>Path to the signed copy of the package</returns>
        public static async Task<string> RepositorySignPackageAsync(
            X509Certificate2 certificate,
            SimpleTestPackageContext nupkg,
            string dir,
            Uri v3ServiceIndex,
            Uri timestampService = null,
            IReadOnlyList<string> packageOwners = null,
            HashAlgorithmName signatureHashAlgorithm = HashAlgorithmName.SHA256,
            HashAlgorithmName timestampHashAlgorithm = HashAlgorithmName.SHA256)
        {
            var signedPackagePath = Path.Combine(dir, $"{nupkg.Id}.{nupkg.Version}.nupkg");
            var tempPath = Path.GetTempFileName();

            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var fileStream = File.OpenWrite(tempPath))
            {
                packageStream.CopyTo(fileStream);
            }

            using (var originalPackage = File.OpenRead(tempPath))
            using (var signedPackage = File.Open(signedPackagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var request = new RepositorySignPackageRequest(
                new X509Certificate2(certificate),
                signatureHashAlgorithm,
                timestampHashAlgorithm,
                v3ServiceIndex,
                packageOwners))
            {
                await CreateSignedPackageAsync(request, originalPackage, signedPackage, timestampService);
            }

            FileUtility.Delete(tempPath);

            return signedPackagePath;
        }

        /// <summary>
        /// Generates an repository signed copy of a package and returns the path to that package
        /// This method can timestamp a package and should only be used with tests marked with [CIOnlyFact]
        /// </summary>
        /// <remarks>If the package is already author signed this method will repository countersign it</remarks>
        /// <param name="certificate">Certificate to be used while signing the package</param>
        /// <param name="packagePath">Package to be signed</param>
        /// <param name="outputDir">Directory for placing the signed package</param>
        /// <param name="timestampService">RFC 3161 timestamp service URL.</param>
        /// <param name="v3ServiceIndex">Value for the V3ServiceIndex for the repository signature attribute</param>
        /// <param name="packageOwners">List of package owners for teh repository signature attribute</param>
        /// <param name="signatureHashAlgorithm">Hash algorithm to be used in the signature. Defaults to SHA256</param>
        /// <param name="timestampHashAlgorithm">Hash algorithm to be used in the timestamp. Defaults to SHA256</param>
        /// <returns>Path to the signed copy of the package</returns>
        public static async Task<string> RepositorySignPackageAsync(
            X509Certificate2 certificate,
            string packagePath,
            string outputDir,
            Uri v3ServiceIndex,
            Uri timestampService = null,
            IReadOnlyList<string> packageOwners = null,
            HashAlgorithmName signatureHashAlgorithm = HashAlgorithmName.SHA256,
            HashAlgorithmName timestampHashAlgorithm = HashAlgorithmName.SHA256)
        {
            var outputPackagePath = Path.Combine(outputDir, Guid.NewGuid().ToString());

            using (var originalPackage = File.OpenRead(packagePath))
            using (var signedPackage = File.Open(outputPackagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var request = new RepositorySignPackageRequest(
                new X509Certificate2(certificate),
                signatureHashAlgorithm,
                timestampHashAlgorithm,
                v3ServiceIndex,
                packageOwners))
            {
                await CreateSignedPackageAsync(request, originalPackage, signedPackage, timestampService);
            }

            return outputPackagePath;
        }
#endif

        public static async Task CreateSignedPackageAsync(
            SignPackageRequest request,
            Stream packageReadStream,
            Stream packageWriteStream,
            Uri timestampService = null)
        {
            Rfc3161TimestampProvider timestampProvider = null;
            if (timestampService != null)
            {
                timestampProvider = new Rfc3161TimestampProvider(timestampService);
            }

            using (var signedPackage = new SignedPackageArchive(packageReadStream, packageWriteStream))
            using (var options = new SigningOptions(
                new Lazy<Stream>(() => packageReadStream),
                new Lazy<Stream>(() => packageWriteStream),
                overwrite: false,
                signatureProvider: new X509SignatureProvider(timestampProvider),
                logger: NullLogger.Instance))
            {
                await SigningUtility.SignAsync(options, request, CancellationToken.None);
            }
        }

        /// <summary>
        /// Generates a Signature for a package.
        /// </summary>
        /// <param name="testCert">Certificate to be used while generating the signature.</param>
        /// <param name="packageStream">Package stream for which the signature has to be generated.</param>
        /// <param name="timestampProvider">An optional timestamp provider.</param>
        /// <returns>Signature for the package.</returns>
        public static async Task<AuthorPrimarySignature> CreateAuthorSignatureForPackageAsync(
            X509Certificate2 testCert,
            Stream packageStream,
            ITimestampProvider timestampProvider = null)
        {
            var hashAlgorithm = HashAlgorithmName.SHA256;
            using (var request = new AuthorSignPackageRequest(testCert, hashAlgorithm))
            using (var package = new PackageArchiveReader(packageStream, leaveStreamOpen: true))
            {
                return (AuthorPrimarySignature)await CreatePrimarySignatureForPackageAsync(package, request, timestampProvider);
            }
        }

        /// <summary>
        /// Generates a Signature for a given package for tests.
        /// </summary>
        /// <param name="package">Package to be used for the signature.</param>
        /// <param name="request">Sign package request for primary signature</param>
        /// <param name="timestampProvider">Provider to add timestamp to package. Defaults to null.</param>
        /// <returns>Signature for the package.</returns>
        public static async Task<PrimarySignature> CreatePrimarySignatureForPackageAsync(
            PackageArchiveReader package,
            SignPackageRequest request,
            ITimestampProvider timestampProvider = null)
        {
            Assert.False(await package.IsSignedAsync(CancellationToken.None));

            var testLogger = new TestLogger();
            var signatureProvider = new X509SignatureProvider(timestampProvider);

            var zipArchiveHash = await package.GetArchiveHashAsync(request.SignatureHashAlgorithm, CancellationToken.None);
            var base64ZipArchiveHash = Convert.ToBase64String(zipArchiveHash);
            var signatureContent = new SignatureContent(SigningSpecifications.V1, request.SignatureHashAlgorithm, base64ZipArchiveHash);

            return await signatureProvider.CreatePrimarySignatureAsync(request, signatureContent, testLogger, CancellationToken.None);
        }

        /// <summary>
        /// Adds a repository countersignature for a given primary signature for tests.
        /// </summary>
        /// <param name="signature">Primary signature to add the repository countersignature.</param>
        /// <param name="request">RepositorySignPackageRequest containing the metadata for the signature request.</param>
        /// <returns>Primary signature with a repository countersignature.</returns>
        public static async Task<PrimarySignature> RepositoryCountersignPrimarySignatureAsync(PrimarySignature signature, RepositorySignPackageRequest request)
        {
            var testLogger = new TestLogger();
            var signatureProvider = new X509SignatureProvider(timestampProvider: null);

            return await signatureProvider.CreateRepositoryCountersignatureAsync(request, signature, testLogger, CancellationToken.None);
        }

#if IS_SIGNING_SUPPORTED
        // This generates a package with a basic signed CMS.
        // The signature MUST NOT have any signed or unsigned attributes.
        public static async Task<FileInfo> SignPackageFileWithBasicSignedCmsAsync(
            TestDirectory directory,
            FileInfo packageFile,
            X509Certificate2 certificate)
        {
            SignatureContent signatureContent;

            using (var stream = packageFile.OpenRead())
            using (var hashAlgorithm = HashAlgorithmName.SHA256.GetHashProvider())
            {
                var hash = hashAlgorithm.ComputeHash(stream, leaveStreamOpen: false);
                signatureContent = new SignatureContent(SigningSpecifications.V1, HashAlgorithmName.SHA256, Convert.ToBase64String(hash));
            }

            var signedPackageFile = new FileInfo(Path.Combine(directory, Guid.NewGuid().ToString()));
            var cmsSigner = new CmsSigner(certificate)
            {
                DigestAlgorithm = HashAlgorithmName.SHA256.ConvertToOid(),
                IncludeOption = X509IncludeOption.WholeChain
            };

            var contentInfo = new ContentInfo(signatureContent.GetBytes());
            var signature = new SignedCms(contentInfo);

            signature.ComputeSignature(cmsSigner);

            Assert.Empty(signature.SignerInfos[0].SignedAttributes);
            Assert.Empty(signature.SignerInfos[0].UnsignedAttributes);

            using (var packageReadStream = packageFile.OpenRead())
            using (var packageWriteStream = signedPackageFile.OpenWrite())
            using (var package = new SignedPackageArchive(packageReadStream, packageWriteStream))
            using (var signatureStream = new MemoryStream(signature.Encode()))
            {
                await package.AddSignatureAsync(signatureStream, CancellationToken.None);
            }

            return signedPackageFile;
        }

        public static Task<PrimarySignature> TimestampSignature(ITimestampProvider timestampProvider, PrimarySignature primarySignature, HashAlgorithmName hashAlgorithm, SignaturePlacement target, ILogger logger)
        {
            Signature signatureToTimestamp = primarySignature;
            if (target == SignaturePlacement.Countersignature)
            {
                signatureToTimestamp = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);
            }

            var signatureValue = signatureToTimestamp.GetSignatureValue();
            var messageHash = hashAlgorithm.ComputeHash(signatureValue);

            var timestampRequest = new TimestampRequest(
                SigningSpecifications.V1,
                messageHash,
                hashAlgorithm,
                target);

            return timestampProvider.TimestampSignatureAsync(primarySignature, timestampRequest, logger, CancellationToken.None);
        }
#endif

        public static async Task<bool> IsSignedAsync(Stream package)
        {
            var currentPosition = package.Position;

            using (var reader = new PackageArchiveReader(package, leaveStreamOpen: true))
            {
                var isSigned = await reader.IsSignedAsync(CancellationToken.None);

                package.Seek(offset: currentPosition, origin: SeekOrigin.Begin);

                return isSigned;
            }
        }

        public static async Task<bool> IsRepositoryCountersignedAsync(Stream package)
        {
            using (var reader = new PackageArchiveReader(package, leaveStreamOpen: true))
            {
                var primarySignature = await reader.GetPrimarySignatureAsync(CancellationToken.None);
                if (primarySignature != null)
                {
#if IS_SIGNING_SUPPORTED
                    return SignatureUtility.HasRepositoryCountersignature(primarySignature);
#endif
                }

                return false;
            }
        }

        public static void TamperWithPackage(string signedPackagePath)
        {
            using (var stream = File.Open(signedPackagePath, FileMode.Open))
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Update))
            {
                zip.Entries.First().Delete();
            }
        }
    }
}
