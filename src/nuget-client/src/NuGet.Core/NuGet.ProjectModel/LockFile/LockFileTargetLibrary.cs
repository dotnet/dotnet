// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Shared;
using NuGet.Versioning;

#nullable enable

namespace NuGet.ProjectModel
{
    public class LockFileTargetLibrary : IEquatable<LockFileTargetLibrary>
    {
        #region Property packing

        private readonly record struct PropertyKey(string PropertyName);

        private static readonly PropertyKey DependenciesKey = new(nameof(Dependencies));
        private static readonly PropertyKey FrameworkAssembliesKey = new(nameof(FrameworkAssemblies));
        private static readonly PropertyKey FrameworkReferencesKey = new(nameof(FrameworkReferences));
        private static readonly PropertyKey RuntimeAssembliesKey = new(nameof(RuntimeAssemblies));
        private static readonly PropertyKey ResourceAssembliesKey = new(nameof(ResourceAssemblies));
        private static readonly PropertyKey CompileTimeAssembliesKey = new(nameof(CompileTimeAssemblies));
        private static readonly PropertyKey NativeLibrariesKey = new(nameof(NativeLibraries));
        private static readonly PropertyKey BuildKey = new(nameof(Build));
        private static readonly PropertyKey BuildMultiTargetingKey = new(nameof(BuildMultiTargeting));
        private static readonly PropertyKey ContentFilesKey = new(nameof(ContentFiles));
        private static readonly PropertyKey RuntimeTargetsKey = new(nameof(RuntimeTargets));
        private static readonly PropertyKey ToolsAssembliesKey = new(nameof(ToolsAssemblies));
        private static readonly PropertyKey EmbedAssembliesKey = new(nameof(EmbedAssemblies));
        private static readonly PropertyKey PackageTypeKey = new(nameof(PackageType));

        private readonly Dictionary<PropertyKey, object> _propertyValues = new();

        private bool _isFrozen;

        private IList<T> GetListProperty<T>(PropertyKey key)
        {
            if (!_propertyValues.TryGetValue(key, out object? value))
            {
                if (_isFrozen)
                {
                    return Array.Empty<T>();
                }

                var list = new List<T>();
                _propertyValues[key] = list;
                return list;
            }

            return (IList<T>)value;
        }

        private void SetListProperty<T>(PropertyKey key, IList<T> list)
        {
            System.Diagnostics.Debug.Assert(!_isFrozen, "Attempting to set a property on a frozen instance.");
            _propertyValues[key] = list;
        }

        public void Freeze()
        {
            _isFrozen = true;
        }

        #endregion

        public string? Name { get; set; }

        public string? Framework { get; set; }

        public NuGetVersion? Version { get; set; }

        public string? Type { get; set; }

        public IList<PackageDependency> Dependencies
        {
            get => GetListProperty<PackageDependency>(DependenciesKey);
            set => SetListProperty(DependenciesKey, value);
        }

        public IList<string> FrameworkAssemblies
        {
            get => GetListProperty<string>(FrameworkAssembliesKey);
            set => SetListProperty(FrameworkAssembliesKey, value);
        }

        public IList<string> FrameworkReferences
        {
            get => GetListProperty<string>(FrameworkReferencesKey);
            set => SetListProperty(FrameworkReferencesKey, value);
        }

        public IList<LockFileItem> RuntimeAssemblies
        {
            get => GetListProperty<LockFileItem>(RuntimeAssembliesKey);
            set => SetListProperty(RuntimeAssembliesKey, value);
        }

        public IList<LockFileItem> ResourceAssemblies
        {
            get => GetListProperty<LockFileItem>(ResourceAssembliesKey);
            set => SetListProperty(ResourceAssembliesKey, value);
        }

        public IList<LockFileItem> CompileTimeAssemblies
        {
            get => GetListProperty<LockFileItem>(CompileTimeAssembliesKey);
            set => SetListProperty(CompileTimeAssembliesKey, value);
        }

        public IList<LockFileItem> NativeLibraries
        {
            get => GetListProperty<LockFileItem>(NativeLibrariesKey);
            set => SetListProperty(NativeLibrariesKey, value);
        }

        public IList<LockFileItem> Build
        {
            get => GetListProperty<LockFileItem>(BuildKey);
            set => SetListProperty(BuildKey, value);
        }

        public IList<LockFileItem> BuildMultiTargeting
        {
            get => GetListProperty<LockFileItem>(BuildMultiTargetingKey);
            set => SetListProperty(BuildMultiTargetingKey, value);
        }

        public IList<LockFileContentFile> ContentFiles
        {
            get => GetListProperty<LockFileContentFile>(ContentFilesKey);
            set => SetListProperty(ContentFilesKey, value);
        }

        public IList<LockFileRuntimeTarget> RuntimeTargets
        {
            get => GetListProperty<LockFileRuntimeTarget>(RuntimeTargetsKey);
            set => SetListProperty(RuntimeTargetsKey, value);
        }

        public IList<LockFileItem> ToolsAssemblies
        {
            get => GetListProperty<LockFileItem>(ToolsAssembliesKey);
            set => SetListProperty(ToolsAssembliesKey, value);
        }

        public IList<LockFileItem> EmbedAssemblies
        {
            get => GetListProperty<LockFileItem>(EmbedAssembliesKey);
            set => SetListProperty(EmbedAssembliesKey, value);
        }

        // PackageType does not belong in Equals and HashCode, since it's only used for compatibility checking post restore.
        public IList<PackageType> PackageType
        {
            get => GetListProperty<PackageType>(PackageTypeKey);
            set => SetListProperty(PackageTypeKey, value);
        }


        public bool Equals(LockFileTargetLibrary? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name, StringComparison.Ordinal)
                && VersionComparer.Default.Equals(Version!, other.Version!)
                && string.Equals(Type, other.Type, StringComparison.Ordinal)
                && string.Equals(Framework, other.Framework, StringComparison.Ordinal)
                && IsListOrderedEqual<PackageDependency>(DependenciesKey, static o => o.Id)
                && IsListOrderedEqual<string>(FrameworkAssembliesKey, static o => o, sequenceComparer: StringComparer.OrdinalIgnoreCase)
                && IsListOrderedEqual<string>(FrameworkReferencesKey, static o => o, sequenceComparer: StringComparer.OrdinalIgnoreCase)
                && IsListOrderedEqual<LockFileItem>(RuntimeAssembliesKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(ResourceAssembliesKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(CompileTimeAssembliesKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(NativeLibrariesKey, static o => o.Path)
                && IsListOrderedEqual<LockFileContentFile>(ContentFilesKey, static o => o.Path)
                && IsListOrderedEqual<LockFileRuntimeTarget>(RuntimeTargetsKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(BuildKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(BuildMultiTargetingKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(ToolsAssembliesKey, static o => o.Path)
                && IsListOrderedEqual<LockFileItem>(EmbedAssembliesKey, static o => o.Path);

            // NOTE we don't include PackageType in Equals or GetHashCode, since it's only used for compatibility checking post restore.

            bool IsListOrderedEqual<T>(PropertyKey key, Func<T, string> accessor, IEqualityComparer<T>? sequenceComparer = null)
            {
                _propertyValues.TryGetValue(key, out object? thisValue);
                other._propertyValues.TryGetValue(key, out object? thatValue);

                IList<T>? thisList = thisValue is IList<T> { Count: not 0 } list1 ? list1 : null;
                IList<T>? thatList = thatValue is IList<T> { Count: not 0 } list2 ? list2 : null;

                return thisList.OrderedEquals<T, string>(thatList, accessor, orderComparer: StringComparer.OrdinalIgnoreCase, sequenceComparer: sequenceComparer);
            }
        }

        public override bool Equals(object? obj)
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
            combiner.AddUnorderedSequence(Dependencies);
            combiner.AddUnorderedSequence(FrameworkAssemblies, StringComparer.OrdinalIgnoreCase);
            combiner.AddUnorderedSequence(FrameworkReferences, StringComparer.OrdinalIgnoreCase);
            combiner.AddUnorderedSequence(RuntimeAssemblies);
            combiner.AddUnorderedSequence(ResourceAssemblies);
            combiner.AddUnorderedSequence(CompileTimeAssemblies);
            combiner.AddUnorderedSequence(NativeLibraries);
            combiner.AddUnorderedSequence(ContentFiles);
            combiner.AddUnorderedSequence(RuntimeTargets);
            combiner.AddUnorderedSequence(Build);
            combiner.AddUnorderedSequence(BuildMultiTargeting);
            combiner.AddUnorderedSequence(ToolsAssemblies);
            combiner.AddUnorderedSequence(EmbedAssemblies);

            return combiner.CombinedHash;

            // NOTE we don't include PackageType in Equals or GetHashCode, since it's only used for compatibility checking post restore.
        }
    }
}
