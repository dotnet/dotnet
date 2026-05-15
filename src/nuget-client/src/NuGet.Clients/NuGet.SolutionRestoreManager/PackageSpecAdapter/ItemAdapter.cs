// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Commands.Restore;

namespace NuGet.SolutionRestoreManager.PackageSpecAdapter
{
    internal class ItemAdapter : IItem
    {
        private readonly IVsReferenceItem2 _item;

        public ItemAdapter(IVsReferenceItem2 item)
        {
            _item = item;
        }

        public string Identity => _item.Name;

        public string? GetMetadata(string name)
        {
            if ((_item.Metadata?.TryGetValue(name, out var value) ?? false) && !string.IsNullOrEmpty(value))
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
