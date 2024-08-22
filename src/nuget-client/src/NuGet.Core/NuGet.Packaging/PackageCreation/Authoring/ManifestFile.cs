// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.PackageCreation.Resources;

namespace NuGet.Packaging
{
    public class ManifestFile
    {
        private static readonly char[] _invalidSourceCharacters = new[] {
                    '\x00', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07',
                    '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F', '\x10', '\x11', '\x12',
                    '\x13', '\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D',
                    '\x1E', '\x1F', '\x22', '\x3C', '\x3E', '\x7C' };
        internal static readonly char[] ReferenceFileInvalidCharacters = _invalidSourceCharacters.Concat(new[] { ':', '*', '?', '\\', '/' }).ToArray();
        private static readonly char[] _invalidTargetChars = ReferenceFileInvalidCharacters.Except(new[] { '\\', '/' }).ToArray();

        private string _target;
        public string Source { get; set; }

        public string Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = string.IsNullOrEmpty(value) ? value : PathUtility.GetPathWithDirectorySeparator(value);
            }
        }

        public string Exclude { get; set; }

        public IEnumerable<string> Validate()
        {
            if (String.IsNullOrEmpty(Source))
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredMetadataMissing, "Source");
            }
            else if (Source.IndexOfAny(_invalidSourceCharacters) != -1)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_SourceContainsInvalidCharacters, Source);
            }

            if (!String.IsNullOrEmpty(Target) && Target.IndexOfAny(_invalidTargetChars) != -1)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_TargetContainsInvalidCharacters, Target);
            }

            if (!String.IsNullOrEmpty(Exclude) && Exclude.IndexOfAny(_invalidSourceCharacters) != -1)
            {
                yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_ExcludeContainsInvalidCharacters, Exclude);
            }
        }
    }
}
