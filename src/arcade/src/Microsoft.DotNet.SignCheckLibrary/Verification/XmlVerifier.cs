// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class XmlVerifier : FileVerifier
    {
        public XmlVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, fileExtension: ".xml")
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            if (VerifyXmlSignatures)
            {
                X509Certificate2 xmlCertificate;
                var svr = new SignatureVerificationResult(path, parent, virtualPath);
                svr.IsSigned = IsSigned(svr.FullPath, out xmlCertificate);
                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);
                return svr;
            }

            return SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
        }

        // See: https://msdn.microsoft.com/en-us/library/ms148731(v=vs.110).aspx
        // See also: https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-verify-the-digital-signatures-of-xml-documents
        // The code differs from the MSDN sample as it checks certificates against the root store instead.
        private bool IsSigned(string path, out X509Certificate2 signingCertificate)
        {
            signingCertificate = null;
            var xmlDoc = new XmlDocument()
            {
                PreserveWhitespace = true
            };

            xmlDoc.Load(path);

            XmlNodeList signatureNodes = xmlDoc.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
            Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.XmlSignatureNodes, signatureNodes.Count);

            if (signatureNodes.Count == 0)
            {
                return false;
            }

            var signedXml = new SignedXml(xmlDoc);
            signedXml.LoadXml((XmlElement)signatureNodes[0]);

            if (signedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>().Count() == 0)
            {
                return false;
            }

            ArrayList certificates = signedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>().First().Certificates;

            foreach (X509Certificate2 certificate in certificates)
            {
                if (signedXml.CheckSignature(certificate, verifySignatureOnly: true))
                {
                    using (var rootStore = new X509Store(StoreName.Root))
                    {
                        rootStore.Open(OpenFlags.IncludeArchived | OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

                        using (var chain = new X509Chain(useMachineContext: true))
                        {
                            chain.Build(certificate);
                            int numberOfChainElements = chain.ChainElements.Count;
                            X509ChainElement rootChainElement = null;
                            X500DistinguishedName subjectDistinguishedName = certificate.SubjectName;

                            // Locate the last element in the chain as that should be the root, otherwise use the certificate we have
                            // and try to match that against a root certificate.
                            if (numberOfChainElements > 0)
                            {
                                rootChainElement = chain.ChainElements[numberOfChainElements - 1];
                                subjectDistinguishedName = rootChainElement.Certificate.SubjectName;
                            }

                            X509Certificate2Collection rootCertificates = rootStore.Certificates;
                            X509Certificate2Collection matchingRootCertificates = rootCertificates.Find(X509FindType.FindBySubjectDistinguishedName,
                                subjectDistinguishedName.Name,
                                validOnly: true);

                            if (matchingRootCertificates.Count > 0)
                            {
                                signingCertificate = matchingRootCertificates[0];
                            }

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
