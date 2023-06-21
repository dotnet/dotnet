// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.Test.Apex.Services;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.UI.TestContract;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Tests.Apex
{
    public class NuGetUIProjectTestExtension : NuGetBaseTestExtension<object, NuGetUIProjectTestExtensionVerifier>
    {
        private ApexTestUIProject _uiproject;
        private TimeSpan _timeout = TimeSpan.FromMinutes(1);
        private ITestLogger _logger;

        public bool IsSolution { get => _uiproject.IsSolution; }

        public NuGetUIProjectTestExtension(ApexTestUIProject project, ITestLogger logger)
        {
            _uiproject = project;
            _logger = logger;
        }

        public bool SearchPackageFromUI(string searchText)
        {
            return _uiproject.WaitForSearchComplete(() => _uiproject.Search(searchText), _timeout);
        }

        public void AssertSearchedPackageItem(string tabName, string packageId, string packageVersion = null)
        {
            var searchPackageResult = _uiproject.VerifyFirstPackageOnTab(tabName, packageId, packageVersion);
            searchPackageResult.Should().BeTrue($"searching for the package {packageId} in the {tabName} tab failed");
        }

        public bool InstallPackageFromUI(string packageId, string version)
        {
            Stopwatch sw = Stopwatch.StartNew();
            bool result = _uiproject.WaitForActionComplete(() => _uiproject.InstallPackage(packageId, version), _timeout);
            sw.Stop();

            _logger.WriteMessage($"{nameof(InstallPackageFromUI)} took {sw.ElapsedMilliseconds}ms to complete");
            return result;
        }

        public bool UninstallPackageFromUI(string packageId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            bool result = _uiproject.WaitForActionComplete(() => _uiproject.UninstallPackage(packageId), _timeout);
            sw.Stop();

            _logger.WriteMessage($"{nameof(UninstallPackageFromUI)} took {sw.ElapsedMilliseconds}ms to complete");
            return result;
        }

        public bool UpdatePackageFromUI(string packageId, string version)
        {
            Stopwatch sw = Stopwatch.StartNew();
            bool result = _uiproject.WaitForActionComplete(
                () => _uiproject.UpdatePackage(new List<PackageIdentity>() { new PackageIdentity(packageId, NuGetVersion.Parse(version)) }),
                _timeout);
            sw.Stop();

            _logger.WriteMessage($"{nameof(UpdatePackageFromUI)} took {sw.ElapsedMilliseconds}ms to complete");
            return result;
        }

        public void SwitchTabToBrowse()
        {
            _uiproject.ActiveFilter = ItemFilter.All;
        }

        public void SwitchTabToInstalled()
        {
            _uiproject.ActiveFilter = ItemFilter.Installed;
        }

        public void SwitchTabToUpdate()
        {
            _uiproject.ActiveFilter = ItemFilter.UpdatesAvailable;
        }

        public void SetPackageSourceOptionToAll()
        {
            _uiproject.SetPackageSourceOptionToAll();
        }
    }
}
