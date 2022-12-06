// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class LockFileTargetLibrary : IEquatable<LockFileTargetLibrary>
    {
        public string Name { get; set; }

        public string Framework { get; set; }

        public NuGetVersion Version { get; set; }

        public string Type { get; set; }

        public IList<PackageDependency> Dependencies { get; set; } = new List<PackageDependency>();

        public IList<string> FrameworkAssemblies { get; set; } = new List<string>();

        public IList<string> FrameworkReferences { get; set; } = new List<string>();

        public IList<LockFileItem> RuntimeAssemblies { get; set; } = new List<LockFileItem>();

        public IList<LockFileItem> ResourceAssemblies { get; set; } = new List<LockFileItem>();

        public IList<LockFileItem> CompileTimeAssemblies { get; set; } = new List<LockFileItem>();

        public IList<LockFileItem> NativeLibraries { get; set; } = new List<LockFileItem>();

        public IList<LockFileItem> Build { get; set; } = new List<LockFileItem>();

        public IList<LockFileItem> BuildMultiTargeting { get; set; } = new List<LockFileItem>();

        public IList<LockFileContentFile> ContentFiles { get; set; } = new List<LockFileContentFile>();

        public IList<LockFileRuntimeTarget> RuntimeTargets { get; set; } = new List<LockFileRuntimeTarget>();

        public IList<LockFileItem> ToolsAssemblies { get; set; } = new List<LockFileItem>();

        public IList<LockFileItem> EmbedAssemblies { get; set; } = new List<LockFileItem>();

        // Package Type does not belong in Equals and HashCode, since it's only used for compatibility checking post restore.
        public IList<PackageType> PackageType { get; set; } = new List<PackageType>();


        public bool Equals(LockFileTargetLibrary other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name, StringComparison.Ordinal)
                && VersionComparer.Default.Equals(Version, other.Version)
                && string.Equals(Type, other.Type, StringComparison.Ordinal)
                && string.Equals(Framework, other.Framework, StringComparison.Ordinal)
                && Dependencies.OrderedEquals(other.Dependencies, dependency => dependency.Id, StringComparer.OrdinalIgnoreCase)
                && FrameworkAssemblies.OrderedEquals(other.FrameworkAssemblies, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase)
                && FrameworkReferences.OrderedEquals(other.FrameworkReferences, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase)
                && RuntimeAssemblies.OrderedEquals(other.RuntimeAssemblies, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && ResourceAssemblies.OrderedEquals(other.ResourceAssemblies, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && CompileTimeAssemblies.OrderedEquals(other.CompileTimeAssemblies, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && NativeLibraries.OrderedEquals(other.NativeLibraries, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && ContentFiles.OrderedEquals(other.ContentFiles, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && RuntimeTargets.OrderedEquals(other.RuntimeTargets, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && Build.OrderedEquals(other.Build, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && BuildMultiTargeting.OrderedEquals(other.BuildMultiTargeting, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && ToolsAssemblies.OrderedEquals(other.ToolsAssemblies, item => item.Path, StringComparer.OrdinalIgnoreCase)
                && EmbedAssemblies.OrderedEquals(other.EmbedAssemblies, item => item.Path, StringComparer.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LockFileTargetLibrary);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Name);
            combiner.AddObject(Version);
            combiner.AddObject(Type);
            combiner.AddObject(Framework);

            foreach (var dependency in Dependencies.OrderBy(dependency => dependency.Id, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(dependency);
            }

            foreach (var reference in FrameworkAssemblies.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddStringIgnoreCase(reference);
            }

            foreach (var reference in FrameworkReferences.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddStringIgnoreCase(reference);
            }

            foreach (var item in RuntimeAssemblies.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in ResourceAssemblies.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in CompileTimeAssemblies.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in NativeLibraries.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in ContentFiles.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in RuntimeTargets.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in Build.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in BuildMultiTargeting.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in ToolsAssemblies.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            foreach (var item in EmbedAssemblies.OrderBy(item => item.Path, StringComparer.OrdinalIgnoreCase))
            {
                combiner.AddObject(item);
            }

            return combiner.CombinedHash;
        }
    }
}
