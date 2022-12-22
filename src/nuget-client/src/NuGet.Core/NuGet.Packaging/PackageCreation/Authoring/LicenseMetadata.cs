// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using NuGet.Packaging.Licenses;
using NuGet.Shared;

namespace NuGet.Packaging
{
    /// <summary>
    /// Represents the Package LicenseMetadata details.
    /// All the relevant warnings and errors should parsed into this model and ideally the readers of this metadata never throw. 
    /// </summary>
    public class LicenseMetadata : IEquatable<LicenseMetadata>
    {
        public static readonly Version EmptyVersion = new Version(1, 0, 0);
        public static readonly Version CurrentVersion = new Version(1, 0, 0);
        public static readonly Uri LicenseFileDeprecationUrl = new Uri("https://aka.ms/deprecateLicenseUrl");
        public static readonly string LicenseServiceLinkTemplate = "https://licenses.nuget.org/{0}";

        /// <summary>
        /// The LicenseType, never null
        /// </summary>
        public LicenseType Type { get; }

        /// <summary>
        /// The license, never null, could be empty.
        /// </summary>
        public string License { get; }

        /// <summary>
        /// The license expression, could be null if the version is higher than the current supported or if the expression is not parseable.
        /// </summary>
        public NuGetLicenseExpression LicenseExpression { get; }

        /// <summary>
        /// Non-null when the expression parsing yielded some issues. This will be used to display the errors/warnings in the UI. Only populated when the metadata element is returned by the nuspec reader;
        /// </summary>
        public IReadOnlyList<string> WarningsAndErrors { get; }

        /// <summary>
        /// LicenseMetadata (expression) version. Never null.
        /// </summary>
        public Version Version { get; }

        public LicenseMetadata(LicenseType type, string license, NuGetLicenseExpression expression, IReadOnlyList<string> warningsAndErrors, Version version)
        {
            Type = type;
            License = license ?? throw new ArgumentNullException(nameof(license));
            LicenseExpression = expression;
            WarningsAndErrors = warningsAndErrors;
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public bool Equals(LicenseMetadata other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return Type == other.Type &&
                   License.Equals(other.License, StringComparison.Ordinal) &&
                   Equals(LicenseExpression, other.LicenseExpression) &&
                   EqualityUtility.SequenceEqualWithNullCheck(WarningsAndErrors, other.WarningsAndErrors) &&
                   Version == other.Version;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LicenseMetadata);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStruct(Type);
            combiner.AddObject(License);
            combiner.AddObject(LicenseExpression);
            combiner.AddSequence(WarningsAndErrors);
            combiner.AddObject(Version);

            return combiner.CombinedHash;
        }

        public Uri LicenseUrl
        {
            get
            {
                switch (Type)
                {
                    case LicenseType.File:
                        return LicenseFileDeprecationUrl;

                    case LicenseType.Expression:
                        return new Uri(GenerateLicenseServiceLink(License));

                    default:
                        return null;
                }
            }
        }

        private static string GenerateLicenseServiceLink(string license)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, LicenseServiceLinkTemplate, license)).AbsoluteUri;
        }
    }

    public enum LicenseType
    {
        File,
        Expression,
    }
}
