// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Microsoft.DotNet.Deployment.Tasks.Links
{
    public abstract class AkaMSLinksBase : Microsoft.Build.Utilities.Task
    {
        [Required]
        // Authentication data
        public string ClientId { get; set; }
        [Required]
        // Authentication data
        public string ClientSecret { get; set; }
        [Required]
        public string Tenant { get; set; }

        public bool UseIdentityClientLibrary { get; set; } = false;
    }
}
