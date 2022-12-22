// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Commands;
using NuGet.Common;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents extension methods to built-in types.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Determines if the specified item has a property value that is equal to <see cref="bool.FalseString" />.
        /// </summary>
        /// <param name="item">The <see cref="IMSBuildItem" /> to get the metadata value from.</param>
        /// <param name="name">The name of the property to get the value of.</param>
        /// <param name="defaultValue">The default value to return if the specified metadata has no value.</param>
        /// <returns><code>true</code> if the specified property value is equal to <see cref="bool.FalseString" />, otherwise <code>false</code>.</returns>
        public static bool IsPropertyFalse(this IMSBuildItem item, string name, bool defaultValue = false)
        {
            return IsValueFalse(item.GetProperty(name), defaultValue);
        }

        /// <summary>
        /// Determines if the specified item has a property value that is equal to <see cref="bool.TrueString" />.
        /// </summary>
        /// <param name="item">The <see cref="IMSBuildItem" /> to get the metadata value from.</param>
        /// <param name="name">The name of the property to get the value of.</param>
        /// <param name="defaultValue">The default value to return if the specified metadata has no value.</param>
        /// <returns><code>true</code> if the specified property value is equal to <see cref="bool.TrueString" />, otherwise <code>false</code>.</returns>
        public static bool IsPropertyTrue(this IMSBuildItem item, string name, bool defaultValue = false)
        {
            return IsValueTrue(item.GetProperty(name), defaultValue);
        }

        /// <summary>
        /// Splits the value of the specified property and returns an array if the property has a value, otherwise returns <code>null</code>.
        /// </summary>
        /// <param name="item">The <see cref="IMSBuildItem" /> to get the property value from.</param>
        /// <param name="name">The name of the property to get the value of and split.</param>
        /// <returns>A <see cref="T:string[]" /> containing the split value of the property if the property had a value, otherwise <code>null</code>.</returns>
        public static string[] SplitPropertyValueOrNull(this IMSBuildItem item, string name)
        {
            string value = item.GetProperty(name);

            return value == null ? null : MSBuildStringUtility.Split(value);
        }

        /// <summary>
        /// Splits the value of the specified property and returns an array if the property has a value, otherwise returns <code>null</code>.
        /// </summary>
        /// <param name="item">The <see cref="IMSBuildItem" /> to get the property value from.</param>
        /// <param name="name">The name of the property to get the value of and split.</param>
        /// <returns>A <see cref="T:string[]" /> containing the split value of the property if the property had a value, otherwise <code>null</code>.</returns>
        public static string[] SplitGlobalPropertyValueOrNull(this IMSBuildProject item, string name)
        {
            string value = item.GetGlobalProperty(name);

            return value == null ? null : MSBuildStringUtility.Split(value);
        }

        /// <summary>
        /// Determines if the specified value is equal to <see cref="bool.FalseString" />.
        /// </summary>
        /// <param name="value">The value to compare to <see cref="bool.FalseString" />.</param>
        /// <param name="defaultValue">The default value to return if the specified value is <code>null</code> or only contains whitespace characters.</param>
        /// <returns><code>true</code> if the specified value is equal to <see cref="bool.FalseString" />, otherwise <code>false</code>.</returns>
        private static bool IsValueFalse(string value, bool defaultValue = false)
        {
            return string.IsNullOrWhiteSpace(value)
                ? defaultValue
                : string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if the specified value is equal to <see cref="bool.TrueString" />.
        /// </summary>
        /// <param name="value">The value to compare to <see cref="bool.TrueString" />.</param>
        /// <param name="defaultValue">The default value to return if the specified value is <code>null</code> or only contains whitespace characters.</param>
        /// <returns><code>true</code> if the specified value is equal to <see cref="bool.TrueString" />, otherwise <code>false</code>.</returns>
        private static bool IsValueTrue(string value, bool defaultValue = false)
        {
            return string.IsNullOrWhiteSpace(value)
                ? defaultValue
                : string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
