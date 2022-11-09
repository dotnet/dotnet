// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace NuGet.Build.Tasks.Test
{
    /// <summary>
    /// Represents an implementation of <see cref="ITaskItem" /> for unit testing.
    /// </summary>
    public sealed class MockTaskItem : Dictionary<string, string>, ITaskItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockTaskItem" /> class.
        /// </summary>
        /// <param name="itemSpec">The item specification which is the value of the Include attribute.</param>
        public MockTaskItem(string itemSpec)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            ItemSpec = itemSpec;
        }

        /// <inheritdoc cref="ITaskItem.ItemSpec" />
        public string ItemSpec { get; set; }

        /// <inheritdoc cref="ITaskItem.MetadataCount" />
        public int MetadataCount => Count;

        /// <inheritdoc cref="ITaskItem.MetadataNames" />
        public ICollection MetadataNames => Keys;

        /// <inheritdoc cref="ITaskItem.CloneCustomMetadata" />
        public IDictionary CloneCustomMetadata()
        {
            return new Dictionary<string, string>(this);
        }

        /// <inheritdoc cref="ITaskItem.CopyMetadataTo(ITaskItem)" />
        public void CopyMetadataTo(ITaskItem destinationItem)
        {
            foreach (var item in this)
            {
                destinationItem.SetMetadata(item.Key, item.Value);
            }
        }

        /// <inheritdoc cref="ITaskItem.GetMetadata(string)" />
        public string GetMetadata(string metadataName)
        {
            string value = string.Empty;

            TryGetValue(metadataName, out value);

            return value;
        }

        /// <inheritdoc cref="ITaskItem.RemoveMetadata(string)" />
        public void RemoveMetadata(string metadataName) => Remove(metadataName);

        /// <inheritdoc cref="ITaskItem.SetMetadata(string, string)" />
        public void SetMetadata(string metadataName, string metadataValue) => this[metadataName] = metadataValue;
    }
}
