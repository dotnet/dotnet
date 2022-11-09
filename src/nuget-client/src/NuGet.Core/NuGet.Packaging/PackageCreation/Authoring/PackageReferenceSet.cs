// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.PackageCreation.Resources;

namespace NuGet.Packaging
{
    public class PackageReferenceSet
    {
        /// <summary>
        /// Creates a new Package Reference Set
        /// </summary>
        /// <param name="references">IEnumerable set of string references</param>
        public PackageReferenceSet(IEnumerable<string> references)
            : this((NuGetFramework)null, references)
        {
        }

        /// <summary>
        /// Creates a new Package Reference Set
        /// </summary>
        /// <param name="targetFramework">The target framework to use, pass Any for AnyFramework. Does not allow null.</param>
        /// <param name="references">IEnumerable set of string references</param>
        public PackageReferenceSet(string targetFramework, IEnumerable<string> references)
            : this(targetFramework != null ? NuGetFramework.Parse(targetFramework) : null, references)
        {
        }

        /// <summary>
        /// Creates a new Package Reference Set
        /// </summary>
        /// <param name="targetFramework">The target framework to use.</param>
        /// <param name="references">IEnumerable set of string references</param>
        public PackageReferenceSet(NuGetFramework targetFramework, IEnumerable<string> references)
        {
            if (references == null)
            {
                throw new ArgumentNullException(nameof(references));
            }

            TargetFramework = targetFramework;
            References = references.ToArray();
        }

        public IReadOnlyCollection<string> References { get; }

        public NuGetFramework TargetFramework { get; }

        public IEnumerable<string> Validate()
        {
            foreach (var reference in References)
            {
                if (String.IsNullOrEmpty(reference))
                {
                    yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_RequiredElementMissing, "File");
                }
                else if (reference.IndexOfAny(ManifestFile.ReferenceFileInvalidCharacters) != -1)
                {
                    yield return String.Format(CultureInfo.CurrentCulture, NuGetResources.Manifest_InvalidReferenceFile, reference);
                }
            }
        }
    }
}
