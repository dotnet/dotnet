// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using Microsoft.SignCheck.Verification;
using Xunit;

namespace Microsoft.DotNet.SignCheckLibrary.Tests;

public class CabSecurityInfoProviderTests
{
    private const string SignerSubject = "CN=SignCheck Test";

    // 36 byte CFHEADER plus cbCFHeader, cbCFFolder and cbCFData
    private const int ReserveFieldsOffset = 40;

    private const ushort CfhdrReservePresent = 0x0004;

    [Fact]
    public void ReadsSignatureFromTheSignedCabinetReservedArea()
    {
        byte[] signature = CreateSignature();
        string path = WriteCabinet(signature, reserveSize: 20);

        try
        {
            SignedCms signedCms = new CabSecurityInfoProvider().ReadSecurityInfo(path);

            Assert.NotNull(signedCms);
            Assert.Single(signedCms.SignerInfos);
            Assert.Equal(SignerSubject, signedCms.SignerInfos[0].Certificate.Subject);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReturnsNullWhenTheReservedAreaIsTooSmallToHoldTheSignatureFields()
    {
        byte[] signature = CreateSignature();
        string path = WriteCabinet(signature, reserveSize: 8);

        try
        {
            Assert.Null(new CabSecurityInfoProvider().ReadSecurityInfo(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReturnsNullWhenTheCabinetHasNoReservedArea()
    {
        byte[] signature = CreateSignature();
        string path = WriteCabinet(signature, reserveSize: 20, reservePresent: false);

        try
        {
            Assert.Null(new CabSecurityInfoProvider().ReadSecurityInfo(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReturnsNullWhenTheFileIsNotACabinet()
    {
        string path = Path.GetTempFileName();
        File.WriteAllBytes(path, new byte[] { 0x4D, 0x5A, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00 });

        try
        {
            Assert.Null(new CabSecurityInfoProvider().ReadSecurityInfo(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Writes a cabinet that carries its signature the way signtool does. The per-cabinet
    /// reserved area holds a 0x00100000 header, then the offset and the size of the signature.
    /// </summary>
    private static string WriteCabinet(byte[] signature, ushort reserveSize, bool reservePresent = true)
    {
        // Keep some bytes between the reserved area and the signature so that reading the
        // offset from the wrong field cannot accidentally land on the signature.
        byte[] body = new byte[512];
        int signatureOffset = ReserveFieldsOffset + reserveSize + body.Length;

        using MemoryStream stream = new();
        using (BinaryWriter writer = new(stream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(0x4643534Du);                                          // signature 'MSCF'
            writer.Write(0u);                                                   // reserved1
            // cbCabinet is the size of the cabinet proper, which excludes the signature that
            // signtool appends after it, so in a signed cabinet it is equal to the signature
            // offset and not to the size of the file. Both sample files agree: cbCabinet is
            // 2843935 and 2844171, and each one matches the offset in the reserved area.
            writer.Write((uint)signatureOffset);                                // cbCabinet
            writer.Write(0u);                                                   // reserved2
            writer.Write((uint)ReserveFieldsOffset);                            // coffFiles
            writer.Write(0u);                                                   // reserved3
            writer.Write((byte)3);                                              // versionMinor
            writer.Write((byte)1);                                              // versionMajor
            writer.Write((ushort)1);                                            // cFolders
            writer.Write((ushort)1);                                            // cFiles
            writer.Write(reservePresent ? CfhdrReservePresent : (ushort)0);     // flags
            writer.Write((ushort)0);                                            // setID
            writer.Write((ushort)0);                                            // iCabinet

            writer.Write(reserveSize);                                          // cbCFHeader
            writer.Write((byte)0);                                              // cbCFFolder
            writer.Write((byte)0);                                              // cbCFData

            // Cabinet fields are little-endian regardless of the machine we run on.
            byte[] reserve = new byte[reserveSize];
            if (reserveSize >= 4)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(reserve.AsSpan(0), 0x00100000u);
            }

            if (reserveSize >= 8)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(reserve.AsSpan(4), (uint)signatureOffset);
            }

            if (reserveSize >= 12)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(reserve.AsSpan(8), (uint)signature.Length);
            }

            writer.Write(reserve);
            writer.Write(body);
            writer.Write(signature);
        }

        string path = Path.GetTempFileName();
        File.WriteAllBytes(path, stream.ToArray());
        return path;
    }

    private static byte[] CreateSignature()
    {
        using RSA rsa = RSA.Create(2048);
        CertificateRequest request = new(SignerSubject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        using X509Certificate2 certificate = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddDays(1));

        // ComputeSignature needs a certificate whose private key can be used for signing,
        // a round trip through PKCS#12 gives us one on every platform.
        using X509Certificate2 signingCertificate = X509CertificateLoader.LoadPkcs12(
            certificate.Export(X509ContentType.Pkcs12),
            password: null,
            X509KeyStorageFlags.Exportable);

        SignedCms signedCms = new(new ContentInfo(new byte[] { 1, 2, 3, 4 }));
        signedCms.ComputeSignature(new CmsSigner(signingCertificate));
        return signedCms.Encode();
    }
}
