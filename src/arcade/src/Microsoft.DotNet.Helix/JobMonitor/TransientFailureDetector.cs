// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Azure;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal static class TransientFailureDetector
    {
        public static bool IsTransient(Exception exception)
        {
            return exception switch
            {
                TaskCanceledException => true,
                TimeoutException => true,
                SocketException => true,
                IOException => true,
                HttpRequestException { StatusCode: null } => true,
                HttpRequestException httpException => IsTransientStatusCode((int)httpException.StatusCode.Value),
                RequestFailedException requestFailedException => IsTransientStatusCode(requestFailedException.Status),
                _ => false,
            };
        }

        private static bool IsTransientStatusCode(int statusCode)
            => statusCode == (int)HttpStatusCode.RequestTimeout
                || statusCode == (int)HttpStatusCode.TooManyRequests
                || statusCode >= 500;
    }
}
