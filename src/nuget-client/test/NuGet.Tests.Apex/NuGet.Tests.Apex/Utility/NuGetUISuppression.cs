// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace NuGet.Tests.Apex
{
    /// <summary>
    /// Suppresses the interactive NuGet Package Manager UI dialogs (the "Preview Changes" window) 
    /// for the lifetime of the instance, then restores the previous registry state on
    /// <see cref="Dispose"/>. Use it in a <c>using</c> around a test so the suppression is scoped to that
    /// test instead of relying on persistent machine state set up elsewhere.
    /// </summary>
    internal sealed class NuGetUISuppression : IDisposable
    {
        private const string NuGetRegistryKey = @"Software\NuGet";
        private const string DoNotShowPreviewWindowRegistryName = "DoNotShowPreviewWindow";
        private const string SuppressUILegalDisclaimerRegistryName = "SuppressUILegalDisclaimer";

        private readonly List<RegistryValueSnapshot> _originalValues = new();

        private NuGetUISuppression()
        {
        }

        public static NuGetUISuppression Suppress()
        {
            var suppression = new NuGetUISuppression();

            using RegistryKey nugetRegistryKey = Registry.CurrentUser.CreateSubKey(NuGetRegistryKey);
            suppression.SuppressValue(nugetRegistryKey, DoNotShowPreviewWindowRegistryName);
            suppression.SuppressValue(nugetRegistryKey, SuppressUILegalDisclaimerRegistryName);

            return suppression;
        }

        private void SuppressValue(RegistryKey nugetRegistryKey, string registryValueName)
        {
            object? originalValue = nugetRegistryKey.GetValue(registryValueName);
            RegistryValueKind originalKind = originalValue is null
                ? RegistryValueKind.None
                : nugetRegistryKey.GetValueKind(registryValueName);

            _originalValues.Add(new RegistryValueSnapshot(registryValueName, originalValue, originalKind));

            nugetRegistryKey.SetValue(registryValueName, "1", RegistryValueKind.String);
        }

        public void Dispose()
        {
            using RegistryKey nugetRegistryKey = Registry.CurrentUser.CreateSubKey(NuGetRegistryKey);

            foreach (RegistryValueSnapshot snapshot in _originalValues)
            {
                if (snapshot.OriginalValue is null)
                {
                    nugetRegistryKey.DeleteValue(snapshot.Name, throwOnMissingValue: false);
                }
                else
                {
                    nugetRegistryKey.SetValue(snapshot.Name, snapshot.OriginalValue, snapshot.Kind);
                }
            }
        }

        private sealed class RegistryValueSnapshot
        {
            public RegistryValueSnapshot(string name, object? originalValue, RegistryValueKind kind)
            {
                Name = name;
                OriginalValue = originalValue;
                Kind = kind;
            }

            public string Name { get; }

            public object? OriginalValue { get; }

            public RegistryValueKind Kind { get; }
        }
    }
}
