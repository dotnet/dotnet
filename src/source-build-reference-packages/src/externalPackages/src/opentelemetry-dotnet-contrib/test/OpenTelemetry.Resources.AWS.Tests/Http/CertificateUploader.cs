// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace OpenTelemetry.Resources.AWS.Tests.Http;

internal class CertificateUploader : IDisposable
{
    private const string CRTHEADER = "-----BEGIN CERTIFICATE-----\n";
    private const string CRTFOOTER = "\n-----END CERTIFICATE-----";
    private string filePath;

    public CertificateUploader()
    {
        this.filePath = Path.GetTempFileName();
    }

    public string FilePath
    {
        get => this.filePath;
        set => this.filePath = value;
    }

    public void Create()
    {
        using var rsa = RSA.Create();
        var certRequest = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        var subjectAlternativeNames = new SubjectAlternativeNameBuilder();
        subjectAlternativeNames.AddDnsName("test");
        certRequest.CertificateExtensions.Add(subjectAlternativeNames.Build());

        // Create a temporary certificate and add validity for 1 day
        var certificate = certRequest.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1));

        var exportData = certificate.Export(X509ContentType.Cert);
        var crt = Convert.ToBase64String(exportData, Base64FormattingOptions.InsertLineBreaks);

        using (var stream = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete))
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(CRTHEADER + crt + CRTFOOTER);
            }
        }
    }

    public void Dispose()
    {
        for (var tries = 0; ; tries++)
        {
            try
            {
                File.Delete(this.filePath);
                return;
            }
            catch (IOException) when (tries < 3)
            {
                // the file is unavailable because it is: still being written to or being processed by another thread
                // sleep for sometime before deleting
                Thread.Sleep(1000);
            }
        }
    }
}

#endif
