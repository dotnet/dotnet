// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.Commands
{
    /// <summary>
    /// Internal ITaskItem abstraction
    /// </summary>
    public class MSBuildItem : IMSBuildItem
    {
        private readonly IDictionary<string, string> _metadata;

        public string Identity { get; }

        public IReadOnlyList<string> Properties
        {
            get
            {
                return _metadata.Keys.AsList();
            }
        }

        public MSBuildItem(string identity, IDictionary<string, string> metadata)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            Identity = identity;
            _metadata = metadata;
        }

        /// <summary>
        /// Get property or null if empty. Trims whitespace from values.
        /// </summary>
        public string GetProperty(string property)
        {
            return GetProperty(property, trim: true);
        }

        /// <summary>
        /// Get property or null if empty.
        /// </summary>
        public string GetProperty(string property, bool trim)
        {
            string val;
            if (_metadata.TryGetValue(property, out val) && val != null)
            {
                if (trim)
                {
                    // Remove whitespace that occurs due to msbuild formatting.
                    val = val.Trim();
                }

                if (string.IsNullOrEmpty(val))
                {
                    // Normalize empty values to null for consistency.
                    val = null;
                }

                return val;
            }

            return null;
        }

        public override string ToString()
        {
            return $"Type: {GetProperty("Type")} Project: {GetProperty("ProjectUniqueName")}";
        }
    }
}
