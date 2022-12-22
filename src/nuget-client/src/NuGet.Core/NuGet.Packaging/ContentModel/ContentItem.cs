// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.ContentModel
{
    public class ContentItem
    {
        private Dictionary<string, object> _properties;
        public string Path { get; set; }

        public Dictionary<string, object> Properties
        {
            get => _properties ?? CreateDictionary();
            internal set => _properties = value;
        }

        public override string ToString()
        {
            return Path;
        }

        private Dictionary<string, object> CreateDictionary()
        {
            var properties = new Dictionary<string, object>();
            _properties = properties;
            return properties;
        }
    }
}
