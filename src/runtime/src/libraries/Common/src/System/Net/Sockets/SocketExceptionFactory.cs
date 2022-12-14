// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
    internal static partial class SocketExceptionFactory
    {
        private static string CreateMessage(int nativeSocketError, EndPoint endPoint)
        {
            return Marshal.GetPInvokeErrorMessage(nativeSocketError) + " " + endPoint.ToString();
        }
    }
}
