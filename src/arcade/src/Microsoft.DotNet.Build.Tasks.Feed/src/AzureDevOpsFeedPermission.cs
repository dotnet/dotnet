// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AzureDevOpsFeedPermission
    {
        public AzureDevOpsFeedPermission(string identityDescriptor, string role)
        {
            IdentityDescriptor = identityDescriptor;
            Role = role;
        }

        public string IdentityDescriptor { get; set; }

        public string Role { get; set; }
    }
}
