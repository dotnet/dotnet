// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using NuGet.Common;

namespace NuGet.Protocol
{
    public static class HttpResponseMessageExtensions
    {
        public static void LogServerWarning(this HttpResponseMessage response, ILogger log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (response.Headers.Contains(ProtocolConstants.ServerWarningHeader))
            {
                foreach (var warning in response.Headers.GetValues(ProtocolConstants.ServerWarningHeader))
                {
                    log.LogWarning(warning);
                }
            }
        }
    }
}
