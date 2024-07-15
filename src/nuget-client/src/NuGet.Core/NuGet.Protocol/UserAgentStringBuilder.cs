// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using NuGet.Packaging;

namespace NuGet.Protocol.Core.Types
{
    public class UserAgentStringBuilder
    {
        public static readonly string DefaultNuGetClientName = "NuGet Client V3";

        private const string UserAgentWithOSDescriptionAndVisualStudioSKUTemplate = "{0}/{1} ({2}, {3})";
        private const string UserAgentWithOSDescriptionTemplate = "{0}/{1} ({2})";
        private const string UserAgentTemplate = "{0}/{1}";

        private readonly string _clientName;
        private string _vsInfo;
        private string _osInfo;

        public UserAgentStringBuilder()
            : this(DefaultNuGetClientName)
        {
        }

        public UserAgentStringBuilder(string clientName)
        {
            _clientName = clientName;

            // Read the client version from the assembly metadata and normalize it.
            NuGetClientVersion = MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString();

            _osInfo = GetOS();
        }

        public string NuGetClientVersion { get; }

        [Obsolete("This value is now ignored")]
        public UserAgentStringBuilder WithOSDescription(string osInfo)
        {
            return this;
        }

        public UserAgentStringBuilder WithVisualStudioSKU(string vsInfo)
        {
            _vsInfo = vsInfo;
            return this;
        }

        public string Build()
        {
            var clientInfo = _clientName;
            if (NuGetTestMode.Enabled)
            {
                clientInfo = NuGetTestMode.NuGetTestClientName;
            }
            else if (string.IsNullOrEmpty(clientInfo))
            {
                clientInfo = DefaultNuGetClientName;
            }

            if (string.IsNullOrEmpty(_osInfo))
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    UserAgentTemplate,
                    clientInfo,
                    NuGetClientVersion);
            }

            if (string.IsNullOrEmpty(_vsInfo))
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    UserAgentWithOSDescriptionTemplate,
                    clientInfo,
                    NuGetClientVersion,
                    _osInfo);
            }
            else
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    UserAgentWithOSDescriptionAndVisualStudioSKUTemplate,
                    _clientName,
                    NuGetClientVersion, /* NuGet version */
                    _osInfo, /* OS version */
                    _vsInfo);  /* VS SKU + version */
            }
        }

        internal static string GetOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows.ToString();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux.ToString();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX.ToString();
            }
#if NETCOREAPP
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSPlatform.FreeBSD.ToString();
            }
            else if (OperatingSystem.IsBrowser())
            {
                return "BROWSER";
            }
#endif
            else
            {
                return "UnknownOS";
            }
        }
    }
}
