// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using NuGet.Commands;

namespace NuGet.Build
{
    /// <summary>
    /// TaskItem wrapper
    /// </summary>
    public class MSBuildTaskItem : IMSBuildItem
    {
        public MSBuildTaskItem(ITaskItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Item = item;
        }

        public ITaskItem Item { get; }

        public string Identity
        {
            get
            {
                return Item.ItemSpec;
            }
        }

        public IReadOnlyList<string> Properties
        {
            get
            {
                return Item.MetadataNames.OfType<string>().ToList();
            }
        }

        public string GetProperty(string property)
        {
            return GetProperty(property, trim: true);
        }

        public string GetProperty(string property, bool trim)
        {
            var val = Item.GetMetadata(property);

            if (trim && val != null)
            {
                // Trim whitespace, MSBuild leaves this in place.
                val = val.Trim();
            }

            if (string.IsNullOrEmpty(val))
            {
                // Normalize empty values to null for consistency.
                return null;
            }

            return val;
        }

        public override string ToString()
        {
            return $"Type: {GetProperty("Type")} Project: {GetProperty("ProjectUniqueName")}";
        }

        public IDictionary CloneCustomMetadata()
        {
            return Item.CloneCustomMetadata();
        }
    }
}
