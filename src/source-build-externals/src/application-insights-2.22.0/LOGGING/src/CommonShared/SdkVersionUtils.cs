// -----------------------------------------------------------------------
// <copyright file="SdkVersionUtils.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Implementation
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    internal class SdkVersionUtils
    {
        internal static string GetSdkVersion(string versionPrefix)
        {
#if NET452 || NET46
            string versionStr = typeof(SdkVersionUtils).Assembly.GetCustomAttributes(false).OfType<AssemblyFileVersionAttribute>().First().Version;
#else
            string versionStr = typeof(SdkVersionUtils).GetTypeInfo().Assembly.GetCustomAttributes<AssemblyFileVersionAttribute>().First().Version;
#endif

            Version version = new Version(versionStr);
            return (versionPrefix ?? string.Empty) + version.ToString(3) + "-" + version.Revision.ToString(CultureInfo.InvariantCulture);
        }
    }
}
