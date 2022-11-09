// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    public class PackArgs
    {
        private string _currentDirectory;
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<string> Arguments { get; set; }
        public string BasePath { get; set; }
        public bool Build { get; set; }
        public IEnumerable<string> Exclude { get; set; }
        public bool ExcludeEmptyDirectories { get; set; }
        public ILogger Logger { get; set; }
        public LogLevel LogLevel { get; set; }
        public bool IncludeReferencedProjects { get; set; }
        public bool InstallPackageToOutputPath { get; set; }
        public IMachineWideSettings MachineWideSettings { get; set; }
        public Version MinClientVersion { get; set; }
        public SymbolPackageFormat SymbolPackageFormat { get; set; } = SymbolPackageFormat.SymbolsNupkg;
        public Lazy<string> MsBuildDirectory { get; set; }
        public bool NoDefaultExcludes { get; set; }
        public bool NoPackageAnalysis { get; set; }
        public string OutputDirectory { get; set; }
        public bool OutputFileNamesWithoutVersion { get; set; }
        public string PackagesDirectory { get; set; }
        public string Path { get; set; }
        public bool Serviceable { get; set; }
        public string SolutionDirectory { get; set; }
        public string Suffix { get; set; }
        public bool Symbols { get; set; }
        public bool Tool { get; set; }
        public string Version { get; set; }
        public bool Deterministic { get; set; }
        public WarningProperties WarningProperties { get; set; }
        public MSBuildPackTargetArgs PackTargetArgs { get; set; }
        public Dictionary<string, string> Properties
        {
            get
            {
                return _properties;
            }
        }

        public string CurrentDirectory
        {
            get
            {
                return _currentDirectory ?? Directory.GetCurrentDirectory();
            }
            set
            {
                _currentDirectory = value;
            }
        }

        public string GetPropertyValue(string propertyName)
        {
            string value;
            if (Properties.TryGetValue(propertyName, out value))
            {
                return value;
            }

            return null;
        }

        public static SymbolPackageFormat GetSymbolPackageFormat(string symbolPackageFormat)
        {
            if (string.Equals(symbolPackageFormat, PackagingConstants.SnupkgFormat, StringComparison.OrdinalIgnoreCase))
            {
                return SymbolPackageFormat.Snupkg;
            }
            else if (string.Equals(symbolPackageFormat, PackagingConstants.SymbolsNupkgFormat, StringComparison.OrdinalIgnoreCase))
            {
                return SymbolPackageFormat.SymbolsNupkg;
            }
            else
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidSymbolPackageFormat, symbolPackageFormat));
            }
        }
    }

    public enum SymbolPackageFormat
    {
        Snupkg,
        SymbolsNupkg
    }
}
