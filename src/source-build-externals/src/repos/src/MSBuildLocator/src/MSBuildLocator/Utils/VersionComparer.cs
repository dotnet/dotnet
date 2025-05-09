// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Build.Locator
{
    internal static class VersionComparer 
    {
        /// <summary>
        /// Determines if both versions are equal.
        /// </summary>
        public static bool Equals(SemanticVersion x, SemanticVersion y) => Compare(x, y) == 0;

        /// <summary>
        /// Compare versions.
        /// </summary>
        public static int Compare(SemanticVersion x, SemanticVersion y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return 0;
            }

            if (Object.ReferenceEquals(y, null))
            {
                return 1;
            }

            if (Object.ReferenceEquals(x, null))
            {
                return -1;
            }

            if (x != null && y != null)
            {
                // compare version
                int result = x.Major.CompareTo(y.Major);
                if (result != 0)
                {
                    return result;
                }                    

                result = x.Minor.CompareTo(y.Minor);
                if (result != 0)
                {
                    return result;
                }

                result = x.Patch.CompareTo(y.Patch);
                if (result != 0)
                {
                    return result;
                }

                // compare release labels
                if (x.IsPrerelease && !y.IsPrerelease)
                {
                    return -1;
                }

                if (!x.IsPrerelease && y.IsPrerelease)
                {
                    return 1;
                }

                if (x.IsPrerelease && y.IsPrerelease)
                {
                    result = CompareReleaseLabels(x.ReleaseLabels, y.ReleaseLabels);
                    if (result != 0)
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Compares sets of release labels.
        /// </summary>
        private static int CompareReleaseLabels(IEnumerable<string> version1, IEnumerable<string> version2)
        {
            int result = 0;

            IEnumerator<string> a = version1.GetEnumerator();
            IEnumerator<string> b = version2.GetEnumerator();

            bool aExists = a.MoveNext();
            bool bExists = b.MoveNext();

            while (aExists || bExists)
            {
                if (!aExists && bExists)
                {
                    return -1;
                } 

                if (aExists && !bExists)
                {
                    return 1;
                }
                    
                result = CompareRelease(a.Current, b.Current);

                if (result != 0)
                {
                    return result;
                }
                    
                aExists = a.MoveNext();
                bExists = b.MoveNext();
            }

            return result;
        }

        /// <summary>
        /// Release labels are compared as numbers if they are numeric, otherwise they will be compared
        /// as strings.
        /// </summary>
        private static int CompareRelease(string version1, string version2)
        {
            int result;

            // check if the identifiers are numeric
            bool v1IsNumeric = Int32.TryParse(version1, out int version1Num);
            bool v2IsNumeric = Int32.TryParse(version2, out int version2Num);

            // if both are numeric compare them as numbers
            if (v1IsNumeric && v2IsNumeric)
            {
                result = version1Num.CompareTo(version2Num);
            }
            else if (v1IsNumeric || v2IsNumeric)
            {
                // numeric labels come before alpha labels
                result = v1IsNumeric ? -1 : 1;
            }
            else
            {
                // Ignoring 2.0.0 case sensitive compare. Everything will be compared case insensitively as 2.0.1 specifies.
                result = StringComparer.OrdinalIgnoreCase.Compare(version1, version2);
            }

            return result;
        }
    }
}
