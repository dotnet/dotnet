// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Commands.Restore;
using NuGet.Commands.Restore.Utility;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;

namespace Test.Utility;

public class TestPackageSpecFactory
{
    private string _fullPath;
    private string _directory;
    private TestTargetFramework _outerBuild;
    private Dictionary<string, ITargetFramework>? _targetFrameworks;

    public static string SampleFullPath { get; } = RuntimeEnvironmentHelper.IsWindows ? "c:\\src\\my.csproj" : "/home/me/src/my.csproj";

    public TestPackageSpecFactory(Action<TargetFrameworkBuilder> builder)
        : this(SampleFullPath, builder)
    {
    }

    public TestPackageSpecFactory(string fullPath, Action<TargetFrameworkBuilder> builder)
        : this(fullPath, System.IO.Path.GetDirectoryName(fullPath) ?? throw new ArgumentException(message: "Could not get directory from path", paramName: nameof(fullPath)), builder)
    {
    }

    public TestPackageSpecFactory(string fullPath, string directory, Action<TargetFrameworkBuilder> builder)
    {
        _fullPath = string.IsNullOrWhiteSpace(fullPath) ? throw new ArgumentException("Must not be null or whitespace", nameof(fullPath)) : fullPath;
        _directory = string.IsNullOrWhiteSpace(directory) ? throw new ArgumentNullException("Must not be null or whitespace", nameof(directory)) : directory;

        var tfBuilder = new TargetFrameworkBuilder(_directory);
        builder(tfBuilder);

        TestTargetFramework outerBuild = tfBuilder.ToTargetFramework();
        if (!outerBuild.Properties.TryGetValue("MSBuildProjectName", out string? msbuildProjectName))
        {
            msbuildProjectName = System.IO.Path.GetFileNameWithoutExtension(_fullPath);
            outerBuild.Properties["MSBuildProjectName"] = msbuildProjectName;
        }

        if (!outerBuild.Properties.TryGetValue(ProjectBuildProperties.MSBuildProjectExtensionsPath, out _))
        {
            outerBuild.Properties[ProjectBuildProperties.MSBuildProjectExtensionsPath] = System.IO.Path.Combine(_directory, "obj");
        }

        string? targetFramework = outerBuild.GetProperty("TargetFramework");
        string? targetFrameworks = outerBuild.GetProperty("TargetFrameworks");
        string? targetFrameworkMoniker = outerBuild.GetProperty(ProjectBuildProperties.TargetFrameworkMoniker);

        if (string.IsNullOrWhiteSpace(targetFramework) && string.IsNullOrWhiteSpace(targetFrameworks) && string.IsNullOrWhiteSpace(targetFrameworkMoniker))
        {
            throw new ArgumentException("TargetFramework, TargetFrameworks, or TargetFrameworkMoniker must be set in the outer build.");
        }
        else if (!string.IsNullOrWhiteSpace(targetFrameworks) && !string.IsNullOrWhiteSpace(targetFramework))
        {
            throw new ArgumentException("Only one of TargetFramework or TargetFrameworks can be set in the outer build.");
        }

        _outerBuild = outerBuild;
    }

    public TestPackageSpecFactory WithInnerBuild(Action<TargetFrameworkBuilder> builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var tfBuilder = new TargetFrameworkBuilder(_directory);
        builder(tfBuilder);

        ITargetFramework innerBuild = tfBuilder.ToTargetFramework(_outerBuild);

        string? targetFramework = innerBuild.GetProperty("TargetFramework");
        if (string.IsNullOrWhiteSpace(targetFramework))
        {
            throw new ArgumentException("TargetFramework must be set in the inner build.");
        }

        _targetFrameworks ??= new Dictionary<string, ITargetFramework>(StringComparer.OrdinalIgnoreCase);

#if NETFRAMEWORK
        if (_targetFrameworks.ContainsKey(targetFramework!))
        {
            throw new InvalidOperationException($"Inner build for target framework '{targetFramework}' is already set.");
        }
        _targetFrameworks[targetFramework!] = innerBuild;
#else
        if (!_targetFrameworks.TryAdd(targetFramework, innerBuild))
        {
            throw new InvalidOperationException($"Inner build for target framework '{targetFramework}' is already set.");
        }
#endif

        _targetFrameworks[targetFramework!] = innerBuild;

        return this;
    }

    public PackageSpec Build(ISettings? settings = null)
    {
        Dictionary<string, ITargetFramework> targetFrameworks;
        if (_targetFrameworks is not null && _targetFrameworks.Count > 0)
        {
            targetFrameworks = _targetFrameworks;
            var targetFrameworksProperty = _outerBuild.GetProperty("TargetFrameworks");
            if (string.IsNullOrWhiteSpace(targetFrameworksProperty))
            {
                throw new InvalidOperationException("TargetFrameworks must be set in the outer build when multiple target frameworks are defined.");
            }

            HashSet<string> aliases = targetFrameworksProperty!
                .Split([';'], StringSplitOptions.RemoveEmptyEntries)
                .Select(alias => alias.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            HashSet<string> missingTargetFrameworks = new HashSet<string>(
                aliases.Where(alias => !targetFrameworks.ContainsKey(alias)),
                StringComparer.OrdinalIgnoreCase);
            HashSet<string> extraTargetFrameworks = new HashSet<string>(
                targetFrameworks.Keys.Where(key => !aliases.Contains(key)),
                StringComparer.OrdinalIgnoreCase);

            if (missingTargetFrameworks.Count > 0 || extraTargetFrameworks.Count > 0)
            {
                string errorMessage = "The TargetFrameworks property does not match the inner builds defined in the PackageSpec.";
                if (missingTargetFrameworks.Count > 0)
                {
                    errorMessage += $" Missing target frameworks: {string.Join(", ", missingTargetFrameworks)}.";
                }
                if (extraTargetFrameworks.Count > 0)
                {
                    errorMessage += $" Extra target frameworks: {string.Join(", ", extraTargetFrameworks)}.";
                }
                throw new InvalidOperationException(errorMessage);
            }
        }
        else
        {
            targetFrameworks = new Dictionary<string, ITargetFramework>(StringComparer.OrdinalIgnoreCase);
            string targetFramework = _outerBuild.GetProperty(ProjectBuildProperties.TargetFramework);
            if (string.IsNullOrWhiteSpace(targetFramework))
            {
                string targetFrameworkMoniker = _outerBuild.GetProperty(ProjectBuildProperties.TargetFrameworkMoniker)
                    ?? throw new InvalidOperationException("TargetFramework or TargetFrameworkMoniker must be set in the outer build.");
                targetFramework = string.Empty;
            }
            targetFrameworks[targetFramework] = _outerBuild;
        }

        var project = new TestProject
        {
            FullPath = _fullPath,
            Directory = _directory,
            OuterBuild = _outerBuild,
            TargetFrameworks = targetFrameworks
        };

        var packageSpec = PackageSpecFactory.GetPackageSpec(project, settings ?? NullSettings.Instance);
        if (packageSpec is null)
        {
            throw new InvalidOperationException("Failed to create PackageSpec from the provided project.");
        }
        return packageSpec;
    }

    public record TargetFrameworkBuilder
    {
        private readonly Dictionary<string, string> _properties = new(StringComparer.OrdinalIgnoreCase);
        private readonly string _projectDirectory;
        private Dictionary<string, List<IItem>>? _items;

        public TargetFrameworkBuilder(string projectDirectory)
        {
            _projectDirectory = projectDirectory;
        }

        public TargetFrameworkBuilder WithProperty(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentNullException(nameof(value)); }

#if NETFRAMEWORK
            if (_properties.ContainsKey(name))
            {
                throw new InvalidOperationException($"Property '{name}' is already defined.");
            }
            _properties[name] = value;
#else
            if (!_properties.TryAdd(name, value))
            {
                throw new InvalidOperationException($"Property '{name}' is already defined.");
            }
#endif

            return this;
        }

        public TargetFrameworkBuilder WithItem(string itemType, string identity, KeyValuePair<string, string>[]? metadata)
        {
            if (string.IsNullOrWhiteSpace(itemType)) { throw new ArgumentNullException(nameof(itemType)); }
            if (string.IsNullOrWhiteSpace(identity)) { throw new ArgumentNullException(nameof(identity)); }

            Dictionary<string, string> itemMetadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in metadata ?? [])
            {
                itemMetadata[kvp.Key] = kvp.Value;
            }

            if (itemType.Equals(ProjectItems.ProjectReference, StringComparison.OrdinalIgnoreCase))
            {
                string fullPath = Path.GetFullPath(Path.Combine(_projectDirectory, identity));
                TryAdd(itemMetadata, "FullPath", fullPath);
            }

            TestItem item = new TestItem
            {
                Identity = identity,
                Metadata = itemMetadata
            };

            if (_items is null)
            {
                _items = new Dictionary<string, List<IItem>>(StringComparer.OrdinalIgnoreCase);
            }

            if (!_items.TryGetValue(itemType, out var itemList))
            {
                itemList = new List<IItem>();
                _items[itemType] = itemList;
            }
            itemList.Add(item);

            return this;

            void TryAdd(Dictionary<string, string> dictionary, string key, string value)
            {
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                }
            }
        }

        internal TestTargetFramework ToTargetFramework(TestTargetFramework outerBuild)
        {
            foreach (var property in outerBuild.Properties)
            {
#if NETFRAMEWORK
                if (!_properties.ContainsKey(property.Key))
                {
                    _properties[property.Key] = property.Value;
                }
#else
                _properties.TryAdd(property.Key, property.Value);
#endif
            }

            if (_items is not null)
            {
                foreach (var item in outerBuild.Items)
                {
                    if (!_items!.ContainsKey(item.Key))
                    {
                        _items[item.Key] = new List<IItem>();
                    }
                    _items[item.Key].AddRange(item.Value);
                }
            }
            else
            {
                _items = outerBuild.Items;
            }

            return ToTargetFramework();
        }

        internal TestTargetFramework ToTargetFramework()
        {
            SimulateDotnetSdk(_properties);

            // Create a new TestTargetFramework with the accumulated properties and items
            return new TestTargetFramework
            {
                Properties = new Dictionary<string, string>(_properties, StringComparer.OrdinalIgnoreCase),
                Items = _items ?? new Dictionary<string, List<IItem>>(StringComparer.OrdinalIgnoreCase)
            };

            void SimulateDotnetSdk(Dictionary<string, string> properties)
            {
                if (!properties.TryGetValue("TargetFramework", out var targetFramework) || string.IsNullOrWhiteSpace(targetFramework))
                {
                    return;
                }

                NuGetFramework? framework = null;
                try
                {
                    framework = NuGetFramework.Parse(targetFramework);
                }
                catch
                {
                }

                if (!properties.TryGetValue("TargetFrameworkIdentifier", out var targetFrameworkIdentifer) && framework is not null)
                {
                    targetFrameworkIdentifer = framework.Framework;
                    properties["TargetFrameworkIdentifier"] = targetFrameworkIdentifer;
                }

                if (!properties.TryGetValue("TargetFrameworkVersion", out var targetFrameworkVersion) && framework is not null)
                {
                    targetFrameworkVersion = $"v{framework.Version.ToString()}";
                    properties["TargetFrameworkVersion"] = targetFrameworkVersion;
                }

                if (!properties.TryGetValue("TargetFrameworkMoniker", out var targetFrameworkMoniker))
                {
                    targetFrameworkMoniker = $"{targetFrameworkIdentifer},Version={targetFrameworkVersion}";
                    properties["TargetFrameworkMoniker"] = targetFrameworkMoniker;
                }

                if (!properties.TryGetValue("TargetPlatformIdentifier", out var targetPlatformIdentifier) && framework is not null)
                {
                    targetPlatformIdentifier = framework.Platform;
                }

                if (!string.IsNullOrWhiteSpace(targetPlatformIdentifier))
                {
                    if (!properties.TryGetValue("TargetPlatformVersion", out var targetPlatformVersion))
                    {
                        targetPlatformVersion = framework?.PlatformVersion?.ToString();
                        targetPlatformVersion = targetPlatformVersion is null ? "v1.0" : $"v{targetPlatformVersion}";
                        properties["TargetPlatformVersion"] = targetPlatformVersion;
                    }

                    if (!properties.TryGetValue("TargetPlatformMoniker", out var targetPlatformMoniker))
                    {
                        targetPlatformMoniker = $"{targetPlatformIdentifier},Version={targetPlatformVersion}";
                        properties["TargetPlatformMoniker"] = targetPlatformMoniker;
                    }
                }

                if (!properties.TryGetValue(ProjectBuildProperties.SdkAnalysisLevel, out _)
                    && framework is not null
                    && framework.Framework.Equals(FrameworkConstants.FrameworkIdentifiers.NetCoreApp, StringComparison.OrdinalIgnoreCase)
                    && framework.Version.Major >= 9)
                {
                    string sdkAnalysisLevel = $"{framework.Version.Major}.0.100";
                    properties[ProjectBuildProperties.SdkAnalysisLevel] = sdkAnalysisLevel;
                }
            }
        }
    }

    private record TestProject : IProject
    {
        public required string FullPath { get; init; }
        public required string Directory { get; init; }
        public required ITargetFramework OuterBuild { get; init; }
        public required IReadOnlyDictionary<string, ITargetFramework> TargetFrameworks { get; init; }
    }

    internal record TestTargetFramework : ITargetFramework
    {
        public required Dictionary<string, string> Properties { get; init; }
        public required Dictionary<string, List<IItem>> Items { get; init; }

        public string GetProperty(string propertyName) => Properties.TryGetValue(propertyName, out var value) ? value : null!;
        public IReadOnlyList<IItem> GetItems(string itemType) => Items.TryGetValue(itemType, out var items) ? items : [];
    }

    internal record TestItem : IItem
    {
        public required string Identity { get; init; }

        internal required IReadOnlyDictionary<string, string> Metadata { get; init; }

        public string GetMetadata(string name)
        {
            if (Metadata.TryGetValue(name, out var value))
            {
                return value;
            }
            return null!;
        }
    }
}
