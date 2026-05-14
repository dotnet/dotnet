// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Win32;

namespace Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

internal interface IWindowsRegistryHelper
{
    IRegistryKey? OpenBaseKey(RegistryHive hKey, RegistryView view);
}

internal interface IRegistryKey : IDisposable
{
    IRegistryKey? OpenSubKey(string name);

    object? GetValue(string name);

    string[]? GetSubKeyNames();
}
