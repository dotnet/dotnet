﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections;
using Microsoft.Build.Framework;

namespace Microsoft.NET.Build.Tasks.UnitTests
{
    /// <summary>
    /// Must be internal to avoid loading msbuild assemblies before the test assembly,
    /// which might need to set the msbuild location.
    /// </summary>
    internal class MockTaskItem : ITaskItem
    {
        private Dictionary<string, string> _metadata = new(StringComparer.OrdinalIgnoreCase);

        public MockTaskItem()
        {
        }

        public MockTaskItem(string itemSpec, Dictionary<string, string> metadata)
        {
            ItemSpec = itemSpec;
            foreach (var m in metadata)
            {
                _metadata.Add(m.Key, m.Value);
            }
        }

        public string ItemSpec { get; set; }

        public int MetadataCount
        {
            get
            {
                return _metadata.Count;
            }
        }

        public ICollection MetadataNames
        {
            get
            {
                return _metadata.Keys;
            }
        }

        public IDictionary CloneCustomMetadata()
        {
            throw new NotImplementedException();
        }

        public void CopyMetadataTo(ITaskItem destinationItem)
        {
            foreach (var kv in _metadata)
            {
                destinationItem.SetMetadata(kv.Key, kv.Value);
            }
        }

        public string GetMetadata(string metadataName)
        {
            string metadataValue = null;
            if (_metadata.TryGetValue(metadataName, out metadataValue))
            {
                return metadataValue ?? string.Empty;
            }

            return string.Empty;
        }

        public void RemoveMetadata(string metadataName)
        {
            string metadataValue = null;
            if (_metadata.TryGetValue(metadataName, out metadataValue))
            {
                _metadata.Remove(metadataName);
            }
        }

        public void SetMetadata(string metadataName, string metadataValue)
        {
            _metadata[metadataName] = metadataValue;
        }
    }
}
