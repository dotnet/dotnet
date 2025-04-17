// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Execution;
using NuGet.Commands.Restore;

namespace NuGet.Build.Tasks.Console
{
    internal class TargetFrameworkAdapter : ITargetFramework
    {
        private ProjectInstance _projectInstance;

        public TargetFrameworkAdapter(ProjectInstance projectInstance)
        {
            _projectInstance = projectInstance;
        }

        public IReadOnlyList<IItem> GetItems(string itemType)
        {
            var items = _projectInstance.GetItems(itemType);
            var list = new List<IItem>();
            foreach (var item in items)
            {
                list.Add(new ItemAdapter(item));
            }
            return list;
        }

        public string GetProperty(string propertyName)
        {
            var value = _projectInstance.GetPropertyValue(propertyName).Trim();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return value;
        }
    }
}
