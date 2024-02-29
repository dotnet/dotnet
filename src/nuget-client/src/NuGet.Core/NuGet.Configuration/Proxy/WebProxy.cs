// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace NuGet.Configuration
{
    /// <summary>
    /// Internal implementation of <see cref="IWebProxy"/> mirroring default desktop one.
    /// Introduced for XPlat coreFx support.
    /// </summary>
    public class WebProxy : IWebProxy
    {
        private IReadOnlyList<string> _bypassList = Array.Empty<string>();

        private Regex[]? _regExBypassList;

        public WebProxy(string proxyAddress)
        {
            if (proxyAddress == null)
            {
                throw new ArgumentNullException(nameof(proxyAddress));
            }

            ProxyAddress = CreateProxyUri(proxyAddress);
            BypassList = Array.Empty<string>();
        }

        public WebProxy(Uri proxyAddress)
        {
            if (proxyAddress == null)
            {
                throw new ArgumentNullException(nameof(proxyAddress));
            }

            ProxyAddress = proxyAddress;
            BypassList = Array.Empty<string>();
        }

        public Uri ProxyAddress { get; private set; }

        public ICredentials? Credentials { get; set; }

        public IReadOnlyList<string> BypassList
        {
            get
            {
                return _bypassList;
            }
            set
            {
                _bypassList = value ?? Array.Empty<string>();

                UpdateRegExList();
            }
        }

        public Uri GetProxy(Uri destination) => ProxyAddress;

        public bool IsBypassed(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (_regExBypassList != null && _regExBypassList.Length > 0)
            {
                var normalizedUri = uri.Scheme + "://" + uri.Host + ((!uri.IsDefaultPort) ? (":" + uri.Port) : "");
                return _regExBypassList.Any(r => r.IsMatch(normalizedUri));
            }

            return false;
        }

        private void UpdateRegExList()
        {
            _regExBypassList = _bypassList?
                .Select(x => WildcardToRegex(x))
                .Select(x => new Regex(x, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                .ToArray();
        }

        private static string WildcardToRegex(string pattern)
        {
            return Regex.Escape(pattern)
                .Replace(@"\*", ".*?")
                .Replace(@"\?", ".");
        }

        private static Uri CreateProxyUri(string address)
        {
            if (address.IndexOf("://", StringComparison.Ordinal) == -1)
            {
                address = "http://" + address;
            }

            return new Uri(address);
        }
    }
}
