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
        public static readonly string CompilerApiVersionProperty = "compilerApiVersion";
        private static readonly object PropertiesLock = new object();

        public LockFileItem(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Path { get; }

        private Dictionary<string, string>? _properties;
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

        public bool Equals(LockFileItem? other)
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
                // Handle null/empty dictionaries (treat them as equal)
                bool thisEmpty = _properties == null || _properties.Count == 0;
                bool otherEmpty = other._properties == null || other._properties.Count == 0;

                if (thisEmpty || otherEmpty)
                {
                    return thisEmpty && otherEmpty;
                }
                else
                {
                    return _properties.OrderedEquals(other._properties, pair => pair.Key, StringComparer.Ordinal);
                }
            }

            return false;
        }

        public override bool Equals(object? obj)
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

        protected string? GetProperty(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Properties.TryGetValue(name, out string? value);
            return value;
        }

        protected void SetProperty(string name, string? value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                Properties.Remove(name);
            }
            else
            {
                Properties[name] = value;
            }
        }
    }
}
