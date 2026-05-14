// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    // This test class is in the SigningTestsCollection because it uses TrustedTestCert,
    // which mutates the shared CodeSigningRootX509Store. Without collection serialization,
    // it can race with other signing tests that build X509 chains using the same store.
    [Collection(SigningTestsCollection.Name)]
    public class PackageArchiveReaderSigningTests
    {
        [NetFxCIOnlyFact]
        public async Task GetContentHash_IsSameForUnsignedAndSignedPackageAsync()
        {
            // this test will create an unsigned package, copy it, then sign it. then compare the contentHash
            var nupkg = new SimpleTestPackageContext("Package.Content.Hash.Test", "1.0.0");

            using (var unsignedDir = TestDirectory.Create())
            {
                var nupkgFileName = $"{nupkg.Identity.Id}.{nupkg.Identity.Version}.nupkg";
                var nupkgFileInfo = await nupkg.CreateAsFileAsync(unsignedDir, nupkgFileName);

                using (var signedDir = TestDirectory.Create())
                {
                    Uri? timestampService = null;
                    var signatureHashAlgorithm = HashAlgorithmName.SHA256;
                    var timestampHashAlgorithm = HashAlgorithmName.SHA256;

                    var signedPackagePath = Path.Combine(signedDir.Path, nupkgFileName);

                    using (var trustedCert = SigningTestUtility.GenerateTrustedTestCertificate())
                    using (var originalPackage = File.OpenRead(nupkgFileInfo.FullName))
                    using (var signedPackage = File.Open(signedPackagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var request = new AuthorSignPackageRequest(
                        trustedCert.Source.Cert,
                        signatureHashAlgorithm,
                        timestampHashAlgorithm))
                    {
                        await SignedArchiveTestUtility.CreateSignedPackageAsync(request, originalPackage, signedPackage, timestampService);
                    }

                    using (var unsignedReader = new PackageArchiveReader(nupkgFileInfo.FullName))
                    using (var signedReader = new PackageArchiveReader(signedPackagePath))
                    {
                        var contentHashUnsigned = unsignedReader.GetContentHash(CancellationToken.None);
                        var contentHashSigned = signedReader.GetContentHash(CancellationToken.None);

                        Assert.Equal(contentHashUnsigned, contentHashSigned);
                    }
                }
            }
        }
    }
}
