// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Core;
using Azure.Storage.Blobs;

namespace Microsoft.DotNet.Helix.Client
{
    class StorageRetryPolicy
    {
        internal static BlobClientOptions GetBlobClientOptionsRetrySettings()
        {
            // If this takes way too long in failure mode, or doesn't retry enough, change the settings here to impact all *BlobHelper classes

            BlobClientOptions options = new BlobClientOptions();
            options.Retry.Delay = TimeSpan.FromSeconds(5);
            options.Retry.MaxDelay = TimeSpan.FromSeconds(30);
            options.Retry.MaxRetries = 10;
            options.Retry.Mode = RetryMode.Exponential;

            return options;
        }
    }
}
