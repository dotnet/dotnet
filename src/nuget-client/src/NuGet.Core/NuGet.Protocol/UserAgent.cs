// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using NuGet.Packaging;

namespace NuGet.Protocol.Core.Types
{
    public static class UserAgent
    {
        static UserAgent()
        {
            // Set default user agent string
            UserAgentString = new UserAgentStringBuilder().Build();
        }

        public static void SetUserAgentString(UserAgentStringBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            UserAgentString = builder.Build();
        }

        public static string UserAgentString { get; private set; }

        /// <summary>
        /// Set user agent string on HttpClient to the static string.
        /// </summary>
        /// <param name="client">Http client</param>
        public static void SetUserAgent(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (!string.IsNullOrEmpty(UserAgentString))
            {
                client.DefaultRequestHeaders.Add("user-agent", UserAgentString);
                client.DefaultRequestHeaders.Add("X-NuGet-Client-Version", MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString());
            }
        }
    }
}
