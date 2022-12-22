// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Win32;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class RegistrySettingUtility
    {
        private const string NuGetRegistryKey = @"Software\NuGet";

        public static void SetBooleanSetting(string key, bool value)
        {
            try
            {
                var nugetRegistrykey = Registry.CurrentUser.CreateSubKey(NuGetRegistryKey);
                nugetRegistrykey.SetValue(
                    key,
                    value ? "1" : "0",
                    RegistryValueKind.String);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the boolean value of a setting from the registry.
        /// </summary>
        /// <param name="key">the name of the registry key.</param>
        /// <returns>True if the value of the registry key is not "0". Otherwise, false.</returns>
        public static bool GetBooleanSetting(string key)
        {
            try
            {
                var nugetRegistrykey = Registry.CurrentUser.OpenSubKey(NuGetRegistryKey);
                var keyValue = nugetRegistrykey == null ?
                    null :
                    nugetRegistrykey.GetValue(key) as string;

                return keyValue != null && keyValue != "0";
            }
            catch
            {
                return false;
            }
        }
    }
}
