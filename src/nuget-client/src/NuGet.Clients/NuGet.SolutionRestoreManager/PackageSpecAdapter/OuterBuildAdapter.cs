// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Commands.Restore;
using NuGet.ProjectManagement;
using NuGet.Shared;

namespace NuGet.SolutionRestoreManager.PackageSpecAdapter
{
    internal class OuterBuildAdapter : ITargetFramework
    {
        public OuterBuildAdapter(string projectDirectory, string projectNameWithoutExtension, IVsProjectRestoreInfo3 nomination)
        {
            _projectDirectory = projectDirectory;
            _projectNameWithoutExtension = projectNameWithoutExtension;
            _nomination = nomination ?? throw new ArgumentNullException(nameof(nomination));
        }

        private readonly string _projectDirectory;
        private readonly string _projectNameWithoutExtension;
        private readonly IVsProjectRestoreInfo3 _nomination;

        public IReadOnlyList<IItem> GetItems(string itemType)
        {
            // CPS nominations don't support "outer builds", so check if the items are the same across all target frameworks.

            int hash = GetItemsHash(itemType, _nomination.TargetFrameworks[0]);
            for (int i = 1; i < _nomination.TargetFrameworks.Count; i++)
            {
                if (GetItemsHash(itemType, _nomination.TargetFrameworks[i]) != hash)
                {
                    string message = string.Format(CultureInfo.InvariantCulture, Resources.ItemValuesAreDifferentAcrossTargetFrameworks, itemType);
                    throw new InvalidOperationException(message);
                }
            }

            List<IItem> result = new List<IItem>();
            if (_nomination.TargetFrameworks[0].Items?.TryGetValue(itemType, out IReadOnlyList<IVsReferenceItem2> items) ?? false)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    result.Add(new ItemAdapter(item));
                }
            }

            return result;

            int GetItemsHash(string itemType, IVsTargetFrameworkInfo4 targetFramework)
            {
                if (targetFramework.Items is not null &&
                    targetFramework.Items.TryGetValue(itemType, out IReadOnlyList<IVsReferenceItem2>? items))
                {
                    HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
                    foreach (var item in items)
                    {
                        hashCodeCombiner.AddObject(item.Name);
                        if (item.Metadata is not null)
                        {
                            foreach (var metadata in item.Metadata.OrderBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase))
                            {
                                hashCodeCombiner.AddObject(metadata.Key);
                                hashCodeCombiner.AddObject(metadata.Value);
                            }
                        }
                    }
                    return hashCodeCombiner.CombinedHash;
                }
                return 0;
            }
        }

        public string? GetProperty(string propertyName)
        {
            var value = VSNominationUtilities.GetSingleNonEvaluatedPropertyOrNull(_nomination.TargetFrameworks, propertyName, str => str);
            if (!string.IsNullOrEmpty(value))
            {
                return value!;
            }

            if (string.Equals(propertyName, ProjectBuildProperties.MSBuildProjectExtensionsPath, StringComparison.OrdinalIgnoreCase))
            {
                var fullPath = Path.GetFullPath(Path.Combine(_projectDirectory, _nomination.MSBuildProjectExtensionsPath));
                return fullPath;
            }

            if (string.Equals(propertyName, ProjectBuildProperties.TargetFrameworks, StringComparison.OrdinalIgnoreCase))
            {
                return _nomination.OriginalTargetFrameworks;
            }

            if (string.Equals(propertyName, ProjectBuildProperties.PackageId, StringComparison.OrdinalIgnoreCase))
            {
                return _projectNameWithoutExtension;
            }

            return null;
        }
    }
}
