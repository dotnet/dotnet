// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security;
using Microsoft.Win32;
using NuGet.Common;

namespace NuGet.CommandLine
{
    public static class RegistryKeyUtility
    {
        public static object GetValueFromRegistryKey(string name, string registryKeyPath, RegistryKey registryKey, ILogger logger)
        {
            try
            {
                using (var key = registryKey?.OpenSubKey(registryKeyPath))
                {
                    var result = key?.GetValue(name);

                    return result;
                }
            }
            catch (SecurityException ex)
            {
                // If the user doesn't have access to the registry, then we'll return null
                ExceptionUtilities.LogException(ex, logger);
                return null;
            }
        }
    }
}
