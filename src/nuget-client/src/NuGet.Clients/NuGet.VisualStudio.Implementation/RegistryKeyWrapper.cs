// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Security;
using Microsoft.Win32;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.VisualStudio
{
    internal class RegistryKeyWrapper : IRegistryKey
    {
        private readonly RegistryKey _registryKey;

        public RegistryKeyWrapper(RegistryHive registryHive, RegistryView registryView = RegistryView.Registry32)
        {
            _registryKey = RegistryKey.OpenBaseKey(registryHive, registryView);
        }

        private RegistryKeyWrapper(RegistryKey registryKey)
        {
            Debug.Assert(registryKey != null);
            _registryKey = registryKey;
        }

        public IRegistryKey OpenSubKey(string name)
        {
            try
            {
                var key = _registryKey.OpenSubKey(name);

                if (key != null)
                {
                    return new RegistryKeyWrapper(key);
                }
            }
            catch (SecurityException ex)
            {
                // If the user doesn't have access to the registry, then we'll return null
                ExceptionHelper.WriteErrorToActivityLog(ex);
            }

            return null;
        }

        public object GetValue(string name)
        {
            try
            {
                return _registryKey.GetValue(name);
            }
            catch (SecurityException ex)
            {
                // If the user doesn't have access to the registry, then we'll return null
                ExceptionHelper.WriteErrorToActivityLog(ex);
                return null;
            }
        }

        public void Close()
        {
            if (_registryKey != null)
            {
                // Note that according to MSDN, this method does nothing if you call it on an instance of RegistryKey that is already closed.
                _registryKey.Close();
            }
        }
    }
}
