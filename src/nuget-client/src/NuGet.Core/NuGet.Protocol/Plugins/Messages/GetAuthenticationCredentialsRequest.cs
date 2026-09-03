// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A get authentication credentials request
    /// </summary>
    public sealed class GetAuthenticationCredentialsRequest
    {
        /// <summary>
        /// Uri
        /// </summary>
        [JsonRequired]
        public Uri Uri { get; }

        /// <summary>
        /// isRetry
        /// </summary>
        [JsonRequired]
        public bool IsRetry { get; }

        /// <summary>
        /// IsNonInteractive - tells the plugin whether it can block the operation to ask for user input. Be it a device flow request or a pop-up. 
        /// </summary>
        [JsonRequired]
        public bool IsNonInteractive { get; }

        /// <summary>
        /// CanShowDialog - tells the plugin whether it can show a dialog if the plugin is run in interactive mode. This being false normally means that the auth method should be device flow.
        /// </summary>
        [JsonRequired]
        public bool CanShowDialog { get; }

        /// <summary>
        /// Create a GetAuthenticationCredentialsRequest
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="isRetry"></param>
        /// <param name="isNonInteractive"></param>
        /// <exception cref="ArgumentNullException"> if <paramref name="uri"/> is null</exception>
        [JsonConstructor]
        public GetAuthenticationCredentialsRequest(Uri uri, bool isRetry, bool isNonInteractive, bool canShowDialog)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            IsRetry = isRetry;
            IsNonInteractive = isNonInteractive;
            CanShowDialog = canShowDialog;
        }
    }
}
