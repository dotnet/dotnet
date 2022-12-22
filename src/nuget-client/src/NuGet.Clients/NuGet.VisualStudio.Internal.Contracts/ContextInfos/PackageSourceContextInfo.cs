// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft;
using NuGet.Configuration;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class PackageSourceContextInfo
    {
        private readonly int _hashCode;

        public PackageSourceContextInfo(string source)
            : this(source, source, isEnabled: true)
        {
        }

        public PackageSourceContextInfo(string source, string name)
            : this(source, name, isEnabled: true)
        {
        }

        public PackageSourceContextInfo(string source, string name, bool isEnabled)
        {
            Assumes.NotNullOrEmpty(name);
            Assumes.NotNullOrEmpty(source);

            Name = name;
            Source = source;
            IsEnabled = isEnabled;

            _hashCode = Name.ToUpperInvariant().GetHashCode() * 3137 + Source.ToUpperInvariant().GetHashCode();
            OriginalHashCode = _hashCode;
        }

        public string Name { get; set; }
        public string Source { get; set; }
        public bool IsMachineWide { get; internal set; }
        public bool IsEnabled { get; set; }
        public string? Description { get; internal set; }
        public int OriginalHashCode { get; internal set; }

        public bool Equals(PackageSourceContextInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase) &&
                Source.Equals(other.Source, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var source = obj as PackageSourceContextInfo;
            if (source != null)
            {
                return Equals(source);
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return Name + " [" + Source + "]";
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public PackageSourceContextInfo Clone()
        {
            return new PackageSourceContextInfo(Source, Name, IsEnabled)
            {
                IsMachineWide = IsMachineWide,
                Description = Description,
                OriginalHashCode = OriginalHashCode,
            };
        }

        public static PackageSourceContextInfo Create(PackageSource packageSource)
        {
            return new PackageSourceContextInfo(packageSource.Source, packageSource.Name, packageSource.IsEnabled)
            {
                IsMachineWide = packageSource.IsMachineWide,
                Description = packageSource.Description,
            };
        }
    }
}
