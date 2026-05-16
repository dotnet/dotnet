// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Commands.Restore;

namespace NuGet.SolutionRestoreManager.PackageSpecAdapter
{
    internal class InnerBuildAdapter : ITargetFramework
    {
        private readonly IVsTargetFrameworkInfo4 _targetFramework;

        public InnerBuildAdapter(IVsTargetFrameworkInfo4 targetFramework)
        {
            _targetFramework = targetFramework;
        }

        public IReadOnlyList<IItem> GetItems(string itemType)
        {
            if (_targetFramework.Items is null ||
                !_targetFramework.Items.TryGetValue(itemType, out IReadOnlyList<IVsReferenceItem2>? items))
            {
                return [];
            }

            List<IItem> result = new List<IItem>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                result.Add(new ItemAdapter(item));
            }
            return result;
        }

        public string? GetProperty(string propertyName)
        {
            if (_targetFramework.Properties.TryGetValue(propertyName, out var value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
