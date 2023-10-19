// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;

namespace NuGet.LibraryModel
{
    public struct LibraryType : IEquatable<LibraryType>
    {
        private static ConcurrentDictionary<string, LibraryType> _knownLibraryTypes = new ConcurrentDictionary<string, LibraryType>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Indicates that the library comes from compiling an XRE-based Project
        /// </summary>
        public static readonly LibraryType Project = Define("project");

        /// <summary>
        /// Indicates that the library comes from compiling an external project (such as an MSBuild-based project)
        /// </summary>
        public static readonly LibraryType ExternalProject = Define("externalProject");

        /// <summary>
        /// Indicates that the library comes from a NuGet Package
        /// </summary>
        public static readonly LibraryType Package = Define("package");

        /// <summary>
        /// Indicates that the library comes from a stand-alone .NET Assembly
        /// </summary>
        public static readonly LibraryType Assembly = Define("assembly");

        /// <summary>
        /// Indicates that the library comes from a .NET Assembly in a globally-accessible
        /// location such as the GAC or the Framework Reference Assemblies
        /// </summary>
        public static readonly LibraryType Reference = Define("reference");

        /// <summary>
        /// Indicates that the library comes from a Windows Metadata Assembly (.winmd file)
        /// </summary>
        public static readonly LibraryType WinMD = Define("winmd");

        /// <summary>
        /// Indicates that the library could not be resolved
        /// </summary>
        public static readonly LibraryType Unresolved = Define("unresolved");

        public string Value { get; }

        public bool IsKnown { get; }

        private LibraryType(string value, bool isKnown)
        {
            Value = value;
            IsKnown = isKnown;
        }

        public static LibraryType Parse(string value)
        {
            LibraryType action;
            if (_knownLibraryTypes.TryGetValue(value, out action))
            {
                return action;
            }
            return new LibraryType(value, false);
        }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(LibraryType other)
        {
            return string.Equals(other.Value, Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is LibraryType && Equals((LibraryType)obj);
        }

        public static bool operator ==(LibraryType left, LibraryType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LibraryType left, LibraryType right)
        {
            return !left.Equals(right);
        }

        public static implicit operator string(LibraryType libraryType)
        {
            return libraryType.Value;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(Value))
            {
                return 0;
            }
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }

        private static LibraryType Define(string name)
        {
            var buildAction = new LibraryType(name, true);
            _knownLibraryTypes[name] = buildAction;
            return buildAction;
        }
    }
}
