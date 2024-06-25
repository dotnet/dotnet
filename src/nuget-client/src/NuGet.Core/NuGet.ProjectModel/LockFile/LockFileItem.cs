// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class LockFileItem : IEquatable<LockFileItem>
    {
        public static readonly string AliasesProperty = "aliases";
        private static readonly object PropertiesLock = new object();

        public LockFileItem(string path)
        {
            Path = path;
        }

        public string Path { get; }

        private Dictionary<string, string> _properties;
        public IDictionary<string, string> Properties
        {
            get
            {
                if (_properties == null)
                {
                    lock (PropertiesLock)
                    {
                        _properties ??= new Dictionary<string, string>();
                    }
                }

                return _properties;
            }
        }

        public override string ToString() => Path;

        public bool Equals(LockFileItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase))
            {
                return Properties.OrderedEquals(other.Properties, pair => pair.Key, StringComparer.Ordinal);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LockFileItem);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(Path);
            combiner.AddDictionary(Properties);

            return combiner.CombinedHash;
        }

        public static implicit operator LockFileItem(string path) => new LockFileItem(path);

        protected string GetProperty(string name)
        {
            string value;
            Properties.TryGetValue(name, out value);
            return value;
        }

        protected void SetProperty(string name, string value)
        {
            Properties[name] = value;
        }
    }
}
