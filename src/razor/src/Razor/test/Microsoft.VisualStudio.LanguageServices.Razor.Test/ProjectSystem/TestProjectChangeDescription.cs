﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

internal class TestProjectChangeDescription : IProjectChangeDescription
{
    public TestProjectChangeDescription(IProjectRuleSnapshot before, IProjectRuleSnapshot after)
    {
        Before = before;
        After = after;

        Difference = Diff.Create(before, after);
    }

    public IProjectRuleSnapshot Before { get; }

    public IProjectChangeDiff Difference { get; }

    public IProjectRuleSnapshot After { get; }

    private class Diff : IProjectChangeDiff
    {
        public static Diff Create(IProjectRuleSnapshot before, IProjectRuleSnapshot after)
        {
            var addedItems = new HashSet<string>(after.Items.Keys);
            addedItems.ExceptWith(before.Items.Keys);

            var removedItems = new HashSet<string>(before.Items.Keys);
            removedItems.ExceptWith(after.Items.Keys);

            // changed items must be present in both sets, but have different properties.
            var changedItems = new HashSet<string>(before.Items.Keys);
            changedItems.IntersectWith(after.Items.Keys);
            changedItems.RemoveWhere(key =>
            {
                var x = before.Items[key];
                var y = after.Items[key];

                if (x.Count != y.Count)
                {
                    return true;
                }

                foreach (var kvp in x)
                {
                    if (!y.Contains(kvp))
                    {
                        return true;
                    }
                }

                return false;
            });

            var changedProperties = new HashSet<string>(before.Properties.Keys);
            changedProperties.RemoveWhere(key =>
            {
                var x = before.Properties[key];
                var y = after.Properties[key];
                return Equals(x, y);
            });

            return new Diff()
            {
                AddedItems = addedItems.ToImmutableHashSet(),
                RemovedItems = removedItems.ToImmutableHashSet(),
                ChangedItems = changedItems.ToImmutableHashSet(),

                // We ignore renamed items.
                RenamedItems = ImmutableDictionary<string, string>.Empty,

                ChangedProperties = changedProperties.ToImmutableHashSet(),
            };
        }

        public IImmutableSet<string> AddedItems { get; private set; }

        public IImmutableSet<string> RemovedItems { get; private set; }

        public IImmutableSet<string> ChangedItems { get; private set; }

        public IImmutableDictionary<string, string> RenamedItems { get; private set; }

        public IImmutableSet<string> ChangedProperties { get; private set; }

        public bool AnyChanges =>
            AddedItems.Count > 0 ||
            RemovedItems.Count > 0 ||
            ChangedItems.Count > 0 ||
            RenamedItems.Count > 0 ||
            ChangedProperties.Count > 0;
    }
}
