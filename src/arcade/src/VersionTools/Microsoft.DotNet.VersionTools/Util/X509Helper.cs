// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;

namespace Microsoft.DotNet.VersionTools.src.Util
{
    class X509Helper
    {
        public static HttpClient GetHttpClientWithCertRevocation()
        {
            return new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true });
        }
    }
}
