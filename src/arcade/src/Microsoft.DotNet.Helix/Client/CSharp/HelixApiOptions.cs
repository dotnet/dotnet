// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Core;
using System;

namespace Microsoft.DotNet.Helix.Client
{
    partial class HelixApiOptions
    {
        // See https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/core/Azure.Core/src/RetryOptions.cs for values this overrides
        public const int DefaultRetryDelaySeconds = 10;
        public const int DefaultMaxRetryCount = 5;

        partial void InitializeOptions()
        {
            if (Credentials != null)
            {
                AddPolicy(new HelixApiTokenAuthenticationPolicy(Credentials), HttpPipelinePosition.PerCall);
            }

            // Users should not generally need to modify these but can do so after creating a HelixApi object if needed
            Retry.Delay = TimeSpan.FromSeconds(DefaultRetryDelaySeconds);
            Retry.MaxRetries = DefaultMaxRetryCount;
        }
    }
}
