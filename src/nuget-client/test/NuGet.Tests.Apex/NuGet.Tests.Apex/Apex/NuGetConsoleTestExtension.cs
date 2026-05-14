// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Console.TestContract;

namespace NuGet.Tests.Apex
{
    public class NuGetConsoleTestExtension : NuGetBaseTestExtension<object, NuGetConsoleTestExtensionVerifier>
    {
        private ApexTestConsole _pmConsole;
        private TimeSpan _timeout = TimeSpan.FromMinutes(5);

        public string ProjectName { get; set; }

        public NuGetConsoleTestExtension(ApexTestConsole console, string projectName)
        {
            _pmConsole = console;
            ProjectName = projectName;
        }

        public void InstallPackageFromPMC(string packageId, string version)
        {
            var command = $"Install-Package {packageId} -ProjectName {ProjectName} -Version {version} ";
            Execute(command);
        }

        public void InstallPackageFromPMC(string packageId, string version, string source)
        {
            var command = $"Install-Package {packageId} -ProjectName {ProjectName} -Version {version} -Source {source}";
            Execute(command);
        }

        public void UninstallPackageFromPMC(string packageId)
        {
            var command = $"Uninstall-Package {packageId} -ProjectName {ProjectName}";
            Execute(command);
        }

        public void UpdatePackageFromPMC(string packageId, string version)
        {
            var command = $"Update-Package {packageId} -ProjectName {ProjectName} -Version {version}";
            Execute(command);
        }

        public void UpdatePackageFromPMC(string packageId, string version, string source)
        {
            var command = $"Update-Package {packageId} -ProjectName {ProjectName} -Version {version} -Source {source}";
            Execute(command);
        }

        public void UpdatePackageFromPMCWithConstraints(string packageId, string flags)
        {
            var command = $"Update-Package {packageId} -ProjectName {ProjectName} {flags}";
            Execute(command);
        }

        public bool IsMessageFoundInPMC(string message)
        {
            return _pmConsole.ConsoleContainsMessage(message);
        }

        public void Execute(string command)
        {
            _pmConsole.RunCommand(command, _timeout);
        }

        public void Clear()
        {
            _pmConsole.Clear();
        }

        public string GetText()
        {
            return _pmConsole.GetText();
        }

        public void SetConsoleWidth(int consoleWidth)
        {
            _pmConsole.SetConsoleWidth(consoleWidth);
        }
    }
}
