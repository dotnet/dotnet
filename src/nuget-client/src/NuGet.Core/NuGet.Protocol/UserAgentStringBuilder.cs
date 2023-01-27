// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
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
        }

        public string NuGetClientVersion { get; }

        public UserAgentStringBuilder WithOSDescription(string osInfo)
        {
#if NETCOREAPP2_0_OR_GREATER
            _osInfo = osInfo.Replace("(", @"\(", StringComparison.Ordinal).Replace(")", @"\)", StringComparison.Ordinal);
#else
            _osInfo = osInfo.Replace("(", @"\(").Replace(")", @"\)");
#endif

            return this;
        }

        public UserAgentStringBuilder WithVisualStudioSKU(string vsInfo)
        {
            _vsInfo = vsInfo;
            return this;
        }

        public string Build()
        {
            var osDescription = _osInfo ?? GetOSVersion();

            var clientInfo = _clientName;
            if (NuGetTestMode.Enabled)
            {
                clientInfo = NuGetTestMode.NuGetTestClientName;
            }
            else if (string.IsNullOrEmpty(clientInfo))
            {
                clientInfo = DefaultNuGetClientName;
            }

            if (string.IsNullOrEmpty(osDescription))
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
                    osDescription);
            }
            else
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    UserAgentWithOSDescriptionAndVisualStudioSKUTemplate,
                    _clientName,
                    NuGetClientVersion, /* NuGet version */
                    osDescription, /* OS version */
                    _vsInfo);  /* VS SKU + version */
            }
        }

        private string GetOSVersion()
        {
            if (_osInfo == null)
            {
#if !IS_CORECLR
                // When not on CoreClr and no OSDescription was provided,
                // we will set it ourselves.
                _osInfo = Environment.OSVersion.ToString();
#else
                // When on CoreClr, one should use the .WithOSDescription() method to set it.
                _osInfo = string.Empty;
#endif
            }

            return _osInfo;
        }
    }
}
