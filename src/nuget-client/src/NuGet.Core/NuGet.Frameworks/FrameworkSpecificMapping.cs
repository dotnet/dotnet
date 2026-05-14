// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Frameworks
{
    /// <summary>
    /// A keyvalue pair specific to a framework identifier
    /// </summary>
    public class FrameworkSpecificMapping
    {
        public FrameworkSpecificMapping(string frameworkIdentifier, string key, string value)
            : this(frameworkIdentifier, new KeyValuePair<string, string>(key, value))
        {
        }

        public FrameworkSpecificMapping(string frameworkIdentifier, KeyValuePair<string, string> mapping)
        {
            FrameworkIdentifier = frameworkIdentifier ?? throw new ArgumentNullException(nameof(frameworkIdentifier));
            Mapping = mapping;
        }

        public string FrameworkIdentifier { get; }

        public KeyValuePair<string, string> Mapping { get; }
    }
}
