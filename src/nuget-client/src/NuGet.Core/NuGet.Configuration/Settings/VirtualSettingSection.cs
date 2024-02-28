// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NuGet.Configuration
{
    public sealed class VirtualSettingSection : SettingSection
    {
        internal VirtualSettingSection(SettingSection section)
            : this(section.ElementName, section.Attributes, section.Items)
        {
        }

        internal VirtualSettingSection(string name, params SettingItem[] children)
            : this(name, attributes: null, children: new HashSet<SettingItem>(children))
        {
        }

        internal VirtualSettingSection(string name, IReadOnlyDictionary<string, string>? attributes, IEnumerable<SettingItem>? children)
            : base(name, attributes, children)
        {
        }

        internal VirtualSettingSection Merge(SettingSection other)
        {
            if (!Equals(other))
            {
                throw new ArgumentException(Resources.Error_MergeTwoDifferentSections);
            }

            foreach (var item in other.Items.Where(item => item != null))
            {
                if (item is ClearItem)
                {
                    if (CanBeCleared)
                    {
                        Children.Clear();
                    }

                    Children.Add(item);

                    continue;
                }

                if (TryGetChild(item, out var currentItem))
                {
                    if (item is UnknownItem unknown)
                    {
                        unknown.Merge((UnknownItem)currentItem);
                    }

                    item.MergedWith = currentItem;
                    Children.Remove(currentItem);
                    Children.Add(item);
                }
                else
                {
                    Children.Add(item);
                }
            }

            return this;
        }

        internal override bool Add(SettingItem setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (!Children.Contains(setting) && !setting.IsEmpty())
            {
                Children.Add(setting);

                return true;
            }

            return false;
        }

        internal override void Remove(SettingItem setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (TryGetChild(setting, out var currentSetting))
            {
                Debug.Assert(!currentSetting.IsAbstract());

                if (currentSetting.Origin != null && currentSetting.Origin.IsMachineWide)
                {
                    throw new InvalidOperationException(Resources.CannotUpdateMachineWide);
                }

                if (currentSetting.Origin != null && currentSetting.Origin.IsReadOnly)
                {
                    throw new InvalidOperationException(Resources.CannotUpdateReadOnlyConfig);
                }

                if (Children.Remove(currentSetting))
                {
                    // Remove it from the appropriate config
                    if (currentSetting.Parent != null && currentSetting.Parent != this)
                    {
                        currentSetting.Parent.Remove(currentSetting);
                    }
                }

                if (currentSetting.MergedWith != null)
                {
                    // Add that back to the set since, we should leave the machine wide setting intact.
                    if (!TryRemoveAllMergedWith(currentSetting, out var undeletedItem))
                    {
                        Children.Add(undeletedItem);
                    }
                }
            }
        }

        private bool TryRemoveAllMergedWith(SettingItem currentSetting, [NotNullWhen(false)] out SettingItem? undeletedItem)
        {
            undeletedItem = null;
            var mergedSettings = new List<SettingItem>();
            var mergedWith = currentSetting.MergedWith;
            while (mergedWith != null)
            {
                mergedSettings.Add(mergedWith);
                mergedWith = mergedWith.MergedWith;
            }

            foreach (var elementToDelete in mergedSettings)
            {
                try
                {
                    elementToDelete.Parent!.Remove(elementToDelete);
                }
                // This means setting was merged with a machine wide settings.
                catch
                {
                    undeletedItem = elementToDelete;
                    return false;
                }
            }

            return true;
        }

        public override SettingBase Clone()
        {
            return new VirtualSettingSection(ElementName, Attributes, Items.Select(s => (SettingItem)s.Clone()));
        }
    }
}
