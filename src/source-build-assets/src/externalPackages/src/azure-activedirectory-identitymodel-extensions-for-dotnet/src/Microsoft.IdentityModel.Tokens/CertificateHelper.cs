// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Helper class to load X509Certificate2 from byte array.
    /// </summary>
    internal class CertificateHelper
    {
        /// <summary>
        /// Load a X509Certificate2 from a base64 encoded string.
        /// </summary>
        public static X509Certificate2 LoadX509Certificate(string data)
        {
#if NET9_0_OR_GREATER
            return X509CertificateLoader.LoadCertificate(Convert.FromBase64String(data));
#else
            return new X509Certificate2(Convert.FromBase64String(data));
#endif
        }
    }
}
