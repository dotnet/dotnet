// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SignCheck.Logging;
using Microsoft.SignCheck.Verification.Jar;

namespace Microsoft.SignCheck.Verification
{
    public class JarVerifier : FileVerifier
    {
        public JarVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, fileExtension: ".jar")
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            if (VerifyJarSignatures)
            {
                var svr = new SignatureVerificationResult(path, parent, virtualPath);

                try
                {
                    JarError.ClearErrors();
                    var jarFile = new JarFile(path);
                    svr.IsSigned = jarFile.IsSigned();

                    if (!svr.IsSigned && JarError.HasErrors())
                    {
                        svr.AddDetail(DetailKeys.Error, JarError.GetLastError());
                    }
                    else
                    {
                        foreach (Timestamp timestamp in jarFile.Timestamps)
                        {
                            svr.AddDetail(DetailKeys.Misc, SignCheckResources.DetailTimestamp, timestamp.SignedOn, timestamp.SignatureAlgorithm);
                        }

                        IEnumerable<Timestamp> invalidTimestamps = from ts in jarFile.Timestamps
                                                                   where !ts.IsValid
                                                                   select ts;

                        foreach (Timestamp ts in invalidTimestamps)
                        {
                            svr.AddDetail(DetailKeys.Error, SignCheckResources.DetailTimestampOutisdeCertValidity, ts.SignedOn, ts.EffectiveDate, ts.ExpiryDate);
                            svr.IsSigned = false;
                        }
                    }

                    svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);
                }
                catch (Exception e)
                {
                    svr.AddDetail(DetailKeys.Error, e.Message);
                }

                return svr;
            }

            return SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
        }
    }
}
