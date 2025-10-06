// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.Resources
{
    /// <summary>Owner Details Uri Template for NuGet V3 HTTP feeds.</summary>
    /// <remarks>Not intended to be created directly. Use <see cref="SourceRepository.GetResourceAsync{T}(System.Threading.CancellationToken)"/>
    /// with <see cref="OwnerDetailsUriTemplateResourceV3"/> for T, and typecast to this class.
    /// </remarks>
    public class OwnerDetailsUriTemplateResourceV3 : INuGetResource
    {
        private readonly string _template;

        private OwnerDetailsUriTemplateResourceV3(string template)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }

        /// <summary>
        /// Creates the specified Owner Details Uri template provided by the server if it exists and is valid.
        /// </summary>
        /// <param name="uriTemplate">The Absolute Uri template provided by the server.</param>
        /// <returns>A valid Owner Details Uri template, or null.</returns>
        public static OwnerDetailsUriTemplateResourceV3? CreateOrNull(Uri uriTemplate)
        {
            if (uriTemplate is null)
            {
                throw new ArgumentNullException(nameof(uriTemplate));
            }

            if (uriTemplate.OriginalString.Length == 0
                || !uriTemplate.IsAbsoluteUri
                || !uriTemplate.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return new OwnerDetailsUriTemplateResourceV3(uriTemplate.OriginalString);
        }

        /// <summary>
        /// Gets a URL for viewing package Owner URL outside of Visual Studio. The URL will not be verified to exist.
        /// </summary>
        /// <param name="owner">The owner username.</param>
        /// <returns>The first URL from the resource, with the URI template applied.</returns>
        public Uri GetUri(string owner)
        {
            var uriString = _template
#if NETCOREAPP
               .Replace("{owner}", owner, StringComparison.OrdinalIgnoreCase);
#else
               .Replace("{owner}", owner);
#endif

            return new Uri(uriString);
        }
    }
}
