// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

#if NETCOREAPP
using System;
#endif
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using NuGet.Common;
using NuGet.Packaging;

namespace NuGet.Protocol.Core.Types
{
    public class UserAgentStringBuilder
    {
        public static readonly string DefaultNuGetClientName = "NuGet Client V3";

        private const string UserAgentWithMetadataTemplate = "{0}/{1} ({2})";
        private const string UserAgentTemplate = "{0}/{1}";

        private readonly string _clientName;
        private string _vsInfo;
        private string _osInfo;
        private string _ciInfo;

        public UserAgentStringBuilder()
            : this(DefaultNuGetClientName)
        {
        }

        public UserAgentStringBuilder(string clientName)
            : this(clientName, EnvironmentVariableWrapper.Instance)
        {
        }

        /// <summary>
        /// Internal constructor for testing purposes that allows injecting an environment variable reader.
        /// </summary>
        /// <param name="clientName">The client name to use in the user agent string.</param>
        /// <param name="environmentVariableReader">The environment variable reader for CI detection.</param>
        internal UserAgentStringBuilder(string clientName, IEnvironmentVariableReader environmentVariableReader)
        {
            _clientName = clientName;

            // Read the client version from the assembly metadata and normalize it.
            NuGetClientVersion = MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString();

            _osInfo = GetOS();
            _ciInfo = CIEnvironmentDetector.Detect(environmentVariableReader);
        }

        public string NuGetClientVersion { get; }

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

            string metadataString = BuildMetadataString();

            if (string.IsNullOrEmpty(metadataString))
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    UserAgentTemplate,
                    clientInfo,
                    NuGetClientVersion);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                UserAgentWithMetadataTemplate,
                clientInfo,
                NuGetClientVersion,
                metadataString);
        }

        /// <summary>
        /// Builds the metadata string for the parentheses section.
        /// Items are collected in order (OS, CI, VS) and joined with ", ".
        /// </summary>
        internal string BuildMetadataString()
        {
            var sb = new StringBuilder();

            // OS info
            if (!string.IsNullOrEmpty(_osInfo))
            {
                sb.Append(_osInfo);
            }

            // CI info (formatted as "CI: {provider}")
            if (!string.IsNullOrEmpty(_ciInfo))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append("CI: ").Append(_ciInfo);
            }

            // VS info
            if (!string.IsNullOrEmpty(_vsInfo))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(_vsInfo);
            }

            return sb.ToString();
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
