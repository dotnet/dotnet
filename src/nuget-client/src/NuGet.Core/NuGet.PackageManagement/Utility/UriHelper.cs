// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NuGet.Configuration;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Static class for UriHelper
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// Open external link
        /// </summary>
        /// <param name="url"></param>
        public static void OpenExternalLink(Uri url)
        {
            if (url == null
                || !url.IsAbsoluteUri)
            {
                return;
            }

            // mitigate security risk
            if (url.IsFile
                || url.IsLoopback
                || url.IsUnc)
            {
                return;
            }

            if (IsHttpUrl(url))
            {
                // REVIEW: Will this allow a package author to execute arbitrary program on user's machine?
                // We have limited the url to be HTTP only, but is it sufficient?
                Process.Start(url.AbsoluteUri);
            }
        }

        /// <summary>
        /// Determine if Http Source via Uri.TryCreate()
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsHttpSource(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }

            Uri uri;
            if (Uri.TryCreate(source, UriKind.Absolute, out uri))
            {
                return IsHttpUrl(uri);
            }
            return false;
        }

        /// <summary>
        /// Determine if active package source is http source
        /// </summary>
        /// <param name="packageSourceProvider"></param>
        /// <returns></returns>
        public static bool IsHttpSource(PackageSourceProvider packageSourceProvider)
        {
            // TODO: Fix the logic here
            var packageSources = packageSourceProvider.LoadPackageSources();
            var activeSource = packageSources.FirstOrDefault();
            if (activeSource == null)
            {
                return false;
            }

            //if (activeSource.IsAggregate())
            if (activeSource.IsEnabled)
            {
                return packageSourceProvider.LoadPackageSources().Any(s => IsHttpSource(s.Source));
            }
            // For API V3, the source could be a local .json file.
            if (activeSource.Source.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return IsHttpSource(activeSource.Source);
        }

        /// <summary>
        /// Determine if source is http source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="packageSourceProvider"></param>
        /// <returns></returns>
        public static bool IsHttpSource(string source, PackageSourceProvider packageSourceProvider)
        {
            if (source != null)
            {
                if (IsHttpSource(source))
                {
                    return true;
                }

                var packageSource = packageSourceProvider.LoadPackageSources()
                    .FirstOrDefault(p => p.Name.Equals(source, StringComparison.OrdinalIgnoreCase));
                return (packageSource != null) && IsHttpSource(packageSource.Source);
            }

            return IsHttpSource(packageSourceProvider);
        }

        private static bool IsHttpUrl(Uri uri)
        {
            return (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private static bool IsLocal(string currentSource)
        {
            Uri currentURI;
            if (Uri.TryCreate(currentSource, UriKind.RelativeOrAbsolute, out currentURI))
            {
                if (currentURI.IsFile)
                {
                    if (Directory.Exists(currentSource))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsUNC(string currentSource)
        {
            Uri currentURI;
            if (Uri.TryCreate(currentSource, UriKind.RelativeOrAbsolute, out currentURI))
            {
                if (currentURI.IsUnc)
                {
                    if (Directory.Exists(currentSource))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determine is any source is local source
        /// </summary>
        /// <param name="packageSourceProvider"></param>
        /// <param name="localSource"></param>
        /// <returns></returns>
        public static bool IsAnySourceLocal(PackageSourceProvider packageSourceProvider, out string localSource)
        {
            localSource = string.Empty;
            if (packageSourceProvider != null)
            {
                //If any of the active sources is local folder and is available, return true
                IEnumerable<PackageSource> sources = null;
                var packageSources = packageSourceProvider.LoadPackageSources();
                var activeSource = packageSources.FirstOrDefault();
                //PackageSource activeSource = packageSourceProvider.ActivePackageSource;

                //if (activeSource.IsAggregate())
                if (activeSource.IsEnabled)
                {
                    sources = packageSourceProvider.LoadPackageSources();
                    foreach (var s in sources)
                    {
                        if (IsLocal(s.Source))
                        {
                            localSource = s.Source;
                            return true;
                        }
                    }
                }
                else
                {
                    if (IsLocal(activeSource.Source))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determine if any source is available
        /// </summary>
        /// <param name="packageSourceProvider"></param>
        /// <param name="checkHttp"></param>
        /// <returns></returns>
        public static bool IsAnySourceAvailable(PackageSourceProvider packageSourceProvider, bool checkHttp)
        {
            //If any of the enabled sources is http, return true
            if (checkHttp)
            {
                bool isHttpSource;
                isHttpSource = IsHttpSource(packageSourceProvider);
                if (isHttpSource)
                {
                    return true;
                }
            }

            if (packageSourceProvider != null)
            {
                //If any of the active sources is UNC share or local folder and is available, return true
                IEnumerable<PackageSource> sources = null;
                //PackageSource activeSource = packageSourceProvider.ActivePackageSource;
                var packageSources = packageSourceProvider.LoadPackageSources();
                var activeSource = packageSources.FirstOrDefault();

                //if (activeSource.IsAggregate())
                if (activeSource.IsEnabled)
                {
                    sources = packageSourceProvider.LoadPackageSources();
                    foreach (var s in sources)
                    {
                        if (IsLocal(s.Source)
                            || IsUNC(s.Source))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (IsLocal(activeSource.Source)
                        || IsUNC(activeSource.Source))
                    {
                        return true;
                    }
                }
            }

            //If none of the above matched, return false
            return false;
        }
    }
}
