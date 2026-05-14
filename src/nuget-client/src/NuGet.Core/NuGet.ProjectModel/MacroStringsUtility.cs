// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.ProjectModel
{
    // Add JsonPackageReader and PackageSpecWriter tests.
    internal static class MacroStringsUtility
    {
        internal const string NUGET_ENABLE_EXPERIMENTAL_MACROS = nameof(NUGET_ENABLE_EXPERIMENTAL_MACROS);
        internal const string UserMacro = "$(User)";

        /// <summary>
        /// Applies macros in place to every string in the list.
        /// Macros are applied only at the beginning of a string.
        /// </summary>
        /// <param name="list">The list of elements to apply the macro on.</param>
        /// <param name="macroValue">The macro value that'll need to match from the strings.</param>
        /// <param name="macroName">The macro that'll replace the value within the string.</param>
        /// <param name="stringComparison">The comparer to use. Normally these strings are paths, so the comparer should be OS aware.</param>
        internal static void ApplyMacros(IList<string> list, string macroValue, string macroName, StringComparison stringComparison)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var result = ApplyMacro(list[i], macroValue, macroName, stringComparison);
                    list[i] = result;
                }
            }
        }

        internal static void ExtractMacros(List<string> list, string macroValue, string macroName)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var result = ExtractMacro(list[i], macroValue, macroName);
                    list[i] = result;
                }
            }
        }

        /// <summary>
        /// Applies macro to the string in the list.
        /// </summary>
        /// <param name="originalString">The string to apply the macro on.</param>
        /// <param name="macroValue">The macro value that'll need to match from the string.</param>
        /// <param name="macroName">The macro that'll replace the value within the string.</param>
        /// <param name="stringComparison">The comparer to use. Normally these strings are paths, so the comparer should be OS aware.</param>
        internal static string ApplyMacro(string originalString, string macroValue, string macroName, StringComparison stringComparison)
        {
            if (!string.IsNullOrEmpty(originalString) && !string.IsNullOrEmpty(macroValue) && originalString.StartsWith(macroValue, stringComparison))
            {
                return macroName + originalString.Substring(macroValue.Length);
            }
            return originalString;
        }

        internal static string ExtractMacro(string originalString, string macroValue, string macroName)
        {
            if (!string.IsNullOrEmpty(originalString) && !string.IsNullOrEmpty(macroName) && originalString.StartsWith(macroName, StringComparison.Ordinal))
            {
                return macroValue + originalString.Substring(macroName.Length);
            }
            return originalString;
        }
    }
}
