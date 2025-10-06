// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Deployment.Tasks.Links
{
    public abstract class AkaMSLinksBase : Microsoft.Build.Utilities.Task
    {
        [Required]
        // Authentication data
        public string ClientId { get; set; }
        // Authentication data
        public string ClientSecret { get; set; }
        public string ClientCertificate { get; set; }
        [Required]
        public string Tenant { get; set; }

        protected AkaMSLinkManager CreateAkaMSLinksManager()
        {
            AkaMSLinkManager manager;
            if (!string.IsNullOrEmpty(ClientCertificate))
            {
#if NET9_0_OR_GREATER
                manager = new AkaMSLinkManager(ClientId, X509CertificateLoader.LoadPkcs12(Convert.FromBase64String(File.ReadAllText(ClientCertificate)), password: null), Tenant, Log);
#else
                manager = new AkaMSLinkManager(ClientId, new X509Certificate2(Convert.FromBase64String(File.ReadAllText(ClientCertificate))), Tenant, Log);
#endif
            }
            else if (!string.IsNullOrEmpty(ClientSecret))
            {
                manager = new AkaMSLinkManager(ClientId, ClientSecret, Tenant, Log);
            }
            else
            {
                throw new Exception("aka.ms Authentication information not provided.");
            }

            return manager;
        }
    }
}
