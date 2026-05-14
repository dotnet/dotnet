// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using Microsoft.Win32;

namespace Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

internal class WindowsRegistryHelper : IWindowsRegistryHelper
{
    public IRegistryKey? OpenBaseKey(RegistryHive hKey, RegistryView view)
    {
        var keyRegistry = RegistryKey.OpenBaseKey(hKey, view);
        return keyRegistry is null ? null : new RegistryKeyWrapper(keyRegistry);
    }
}

internal class RegistryKeyWrapper : IRegistryKey
{
    private readonly RegistryKey _registryKey;

    public RegistryKeyWrapper(RegistryKey registryKey)
    {
        _registryKey = registryKey;
    }

    public object? GetValue(string name)
    {
        return _registryKey?.GetValue(name)?.ToString();
    }

    public IRegistryKey? OpenSubKey(string name)
    {
        var keyRegistry = _registryKey.OpenSubKey(name);
        return keyRegistry is null ? null : new RegistryKeyWrapper(keyRegistry);
    }

    public string[]? GetSubKeyNames()
        => _registryKey?.GetSubKeyNames();

    public void Dispose()
    {
        _registryKey?.Dispose();
    }
}
