// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;

namespace Microsoft.DotNet.VersionTools.Automation
{
    internal class ClientHelpers
    {
        public static string ToBase64(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        public static string FromBase64(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
    }
}
