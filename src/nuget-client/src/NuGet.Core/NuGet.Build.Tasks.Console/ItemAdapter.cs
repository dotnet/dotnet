// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Build.Execution;
using NuGet.Commands.Restore;

namespace NuGet.Build.Tasks.Console
{
    internal class ItemAdapter : IItem
    {
        private ProjectItemInstance _item;

        public ItemAdapter(ProjectItemInstance item)
        {
            _item = item;
        }

        public string Identity => _item.EvaluatedInclude;

        public string GetMetadata(string name)
        {
            var value = _item.GetMetadataValue(name).Trim();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return value;
        }
    }
}
