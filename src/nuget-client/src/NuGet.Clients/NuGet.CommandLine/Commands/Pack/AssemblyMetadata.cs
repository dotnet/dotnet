// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.CommandLine
{
    [Serializable]
    public class AssemblyMetadata
    {
        public AssemblyMetadata(Dictionary<string, string> properties = null)
        {
            Properties = properties ??
                // Just like parameter replacements, these are also case insensitive, for consistency.
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string InformationalVersion { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string Copyright { get; set; }

        /// <summary>
        /// Supports extra metadata properties specified for an assembly 
        /// using AssemblyMetadataAttribute.
        /// </summary>
        public Dictionary<string, string> Properties { get; }
    }
}
