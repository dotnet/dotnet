// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class PackageDetailsUriResourceV3 : INuGetResource
    {
        private readonly string _template;

        private PackageDetailsUriResourceV3(string template)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public static PackageDetailsUriResourceV3 CreateOrNull(string uriTemplate)
        {
            if (string.IsNullOrWhiteSpace(uriTemplate)
                || !IsValidUriTemplate(uriTemplate))
            {
                return null;
            }

            return new PackageDetailsUriResourceV3(uriTemplate);
        }

        private static bool IsValidUriTemplate(string uriTemplate)
        {
            Uri uri;
            var isValidUri = Uri.TryCreate(uriTemplate, UriKind.Absolute, out uri);

            // Only allow HTTPS package details URLs.
            if (isValidUri && !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return isValidUri;
        }

        /// <summary>
        /// Gets a URL for viewing package details outside of Visual Studio. The URL will not be verified to exist.
        /// </summary>
        /// <param name="id">The package id (any casing).</param>
        /// <param name="version">The package version.</param>
        /// <returns>The first URL from the resource, with the URI template applied.</returns>
        public Uri GetUri(string id, NuGetVersion version)
        {
            var uriString = _template
#if NETCOREAPP
               .Replace("{id}", id, StringComparison.OrdinalIgnoreCase)
               .Replace("{version}", version.ToNormalizedString(), StringComparison.OrdinalIgnoreCase);
#else
               .Replace("{id}", id)
               .Replace("{version}", version.ToNormalizedString());
#endif

            return new Uri(uriString);
        }
    }
}
