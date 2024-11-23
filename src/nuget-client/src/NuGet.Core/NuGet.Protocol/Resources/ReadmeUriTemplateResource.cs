// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

#if NETCOREAPP
using System;
#endif

namespace NuGet.Protocol
{
    /// <summary>
    /// A resource that provides the URI for downloading a README file based on a template.
    /// </summary>
    internal class ReadmeUriTemplateResource : INuGetResource
    {
        private readonly string _uriTemplate;
        private const string LowerId = "{lower_id}";
        private const string LowerVersion = "{lower_version}";

        public ReadmeUriTemplateResource(string uriTemplate)
        {
            _uriTemplate = uriTemplate;
        }

        /// <summary>
        /// Get the URL for downloading the readme file.
        /// </summary>
        /// <param name="id">The package id</param>
        /// <param name="version">The package version</param>
        /// <returns>URL to download README, built using the URI template.</returns>
        public string GetReadmeUrl(string id, NuGetVersion version)
        {
            if (_uriTemplate == null)
            {
                return string.Empty;
            }

            var uriString = _uriTemplate
#if NETCOREAPP
               .Replace(LowerId, id.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase)
               .Replace(LowerVersion, version.ToNormalizedString().ToLowerInvariant(), StringComparison.OrdinalIgnoreCase);
#else
               .Replace(LowerId, id.ToLowerInvariant())
               .Replace(LowerVersion, version.ToNormalizedString().ToLowerInvariant());
#endif

            return uriString;
        }
    }
}
