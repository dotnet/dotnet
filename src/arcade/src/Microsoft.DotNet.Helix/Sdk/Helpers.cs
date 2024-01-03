// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Reflection;

namespace Microsoft.DotNet.Helix.Sdk
{
    public static class Helpers
    {
        public static ProductInfoHeaderValue UserAgentHeaderValue =>
            new ProductInfoHeaderValue(new ProductHeaderValue("HelixSdk", ProductVersion));

        public static string ProductVersion { get; } =
            typeof(Helpers).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public static string CleanWorkItemName(string workItemName)
        {
            var convertedName = workItemName.Replace('/', '-');
            return convertedName;
        }
    }
}
