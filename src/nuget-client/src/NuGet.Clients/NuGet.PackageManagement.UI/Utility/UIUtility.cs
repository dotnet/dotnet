// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using ContractsItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.UI
{
    public static class UIUtility
    {
        public static void LaunchExternalLink(Uri url)
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
                // If browser has issues opening the url, unhandled exceptions may crash VS
                try
                {
                    Process.Start(url.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    ActivityLog.LogError(NuGetUI.LogEntrySource, ex.Message);
                }
            }
        }

        private static bool IsHttpUrl(Uri uri)
        {
            return (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        // Convert numbers into strings like "1.2K", "33.4M" etc.
        // Precondition: number > 0.
        public static string NumberToString(long number, IFormatProvider culture)
        {
            double RoundDown(double num, long precision)
            {
                double val = num / precision;
                return val > 999 ? 999 : val;
            }

            const long thousand = 1_000L;
            const long million = 1_000_000L;
            const long billion = 1_000_000_000L;
            const long trillion = 1_000_000_000_000L;

            if (number < thousand)
            {
                return number.ToString("G0", culture);
            }

            if (number < million)
            {
                return string.Format(culture, Resources.Thousand, RoundDown(number, thousand));
            }

            if (number < billion)
            {
                return string.Format(culture, Resources.Million, RoundDown(number, million));
            }

            if (number < trillion)
            {
                return string.Format(culture, Resources.Billion, RoundDown(number, billion));
            }

            return string.Format(culture, Resources.Trillion, RoundDown(number, trillion));
        }

        public static string CreateSearchQuery(string query)
        {
            return "packageid:" + query;
        }

        public static ContractsItemFilter ToContractsItemFilter(ItemFilter filter)
        {
            switch (filter)
            {
                case ItemFilter.All:
                    return ContractsItemFilter.All;
                case ItemFilter.Consolidate:
                    return ContractsItemFilter.Consolidate;
                case ItemFilter.Installed:
                    return ContractsItemFilter.Installed;
                case ItemFilter.UpdatesAvailable:
                    return ContractsItemFilter.UpdatesAvailable;
                default:
                    break;
            }

            return ContractsItemFilter.All;
        }
    }
}
