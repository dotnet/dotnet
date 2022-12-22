// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class ReportAbuseResourceV3 : INuGetResource
    {
        private readonly string _uriTemplate;

        public ReportAbuseResourceV3(string uriTemplate)
        {
            if (string.IsNullOrEmpty(uriTemplate) || !IsValidUriTemplate(uriTemplate))
            {
                // fallback to default nuget.org ReportAbuseUriTemplate
                _uriTemplate = "https://www.nuget.org/packages/{id}/{version}/ReportAbuse";
            }
            else
            {
                _uriTemplate = uriTemplate;
            }
        }

        /// <summary>
        /// Gets a URL for reporting package abuse. The URL will not be verified to exist.
        /// </summary>
        /// <param name="id">The package id (natural casing)</param>
        /// <param name="version">The package version</param>
        /// <returns>The first URL from the resource, with the URI template applied.</returns>
        public Uri GetReportAbuseUrl(string id, NuGetVersion version)
        {
            var uriString = _uriTemplate
#if NETCOREAPP
               .Replace("{id}", id, StringComparison.OrdinalIgnoreCase)
               .Replace("{version}", version.ToNormalizedString(), StringComparison.OrdinalIgnoreCase);
#else
               .Replace("{id}", id)
               .Replace("{version}", version.ToNormalizedString());
#endif

            return new Uri(uriString);
        }

        private static bool IsValidUriTemplate(string uriTemplate)
        {
            Uri uri;
            var isValidUri = Uri.TryCreate(uriTemplate, UriKind.Absolute, out uri);
            return isValidUri;
        }
    }
}
