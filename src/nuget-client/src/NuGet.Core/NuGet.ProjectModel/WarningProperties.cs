// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Class to hold warning properties given by project system.
    /// </summary>
    public class WarningProperties : IEquatable<WarningProperties>
    {
        /// <summary>
        /// List of Warning Codes that should be treated as Errors.
        /// </summary>
        public ISet<NuGetLogCode> WarningsAsErrors { get; }

        /// <summary>
        /// List of Warning Codes that should be ignored.
        /// </summary>
        public ISet<NuGetLogCode> NoWarn { get; }

        /// <summary>
        /// Indicates if all warnings should be ignored.
        /// </summary>
        public bool AllWarningsAsErrors { get; set; }

        /// <summary>
        /// List of Warning Codes that should not be treated as Errors.
        /// </summary>
        public ISet<NuGetLogCode> WarningsNotAsErrors { get; }

        public WarningProperties()
        {
            WarningsAsErrors = new HashSet<NuGetLogCode>();
            NoWarn = new HashSet<NuGetLogCode>();
            AllWarningsAsErrors = false;
            WarningsNotAsErrors = new HashSet<NuGetLogCode>();
        }

        [Obsolete("Use the constructor with 4 instead.")]
        public WarningProperties(ISet<NuGetLogCode> warningsAsErrors, ISet<NuGetLogCode> noWarn, bool allWarningsAsErrors)
            : this(warningsAsErrors, noWarn, allWarningsAsErrors, new HashSet<NuGetLogCode>())
        {
        }

        public WarningProperties(ISet<NuGetLogCode> warningsAsErrors, ISet<NuGetLogCode> noWarn, bool allWarningsAsErrors, ISet<NuGetLogCode> warningsNotAsErrors)
        {
            WarningsAsErrors = warningsAsErrors ?? throw new ArgumentNullException(nameof(warningsAsErrors));
            NoWarn = noWarn ?? throw new ArgumentNullException(nameof(noWarn));
            AllWarningsAsErrors = allWarningsAsErrors;
            WarningsNotAsErrors = warningsNotAsErrors ?? throw new ArgumentNullException(nameof(warningsNotAsErrors));
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(AllWarningsAsErrors);
            hashCode.AddSequence(WarningsAsErrors.OrderBy(e => e));
            hashCode.AddSequence(NoWarn.OrderBy(e => e));
            hashCode.AddSequence(WarningsNotAsErrors.OrderBy(e => e));

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WarningProperties);
        }

        public bool Equals(WarningProperties other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return AllWarningsAsErrors == other.AllWarningsAsErrors &&
                EqualityUtility.SetEqualsWithNullCheck(WarningsAsErrors, other.WarningsAsErrors) &&
                EqualityUtility.SetEqualsWithNullCheck(NoWarn, other.NoWarn) &&
                EqualityUtility.SetEqualsWithNullCheck(WarningsNotAsErrors, other.WarningsNotAsErrors);
        }

        public WarningProperties Clone()
        {
            return new WarningProperties(warningsAsErrors: new HashSet<NuGetLogCode>(WarningsAsErrors), noWarn: new HashSet<NuGetLogCode>(NoWarn), allWarningsAsErrors: AllWarningsAsErrors, warningsNotAsErrors: WarningsNotAsErrors);
        }

        /// <summary>
        /// Create warning properties from the msbuild property strings.
        /// </summary>
        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, string warningsAsErrors, string noWarn, string warningsNotAsErrors)
        {
            return GetWarningProperties(treatWarningsAsErrors, MSBuildStringUtility.GetNuGetLogCodes(warningsAsErrors), MSBuildStringUtility.GetNuGetLogCodes(noWarn), MSBuildStringUtility.GetNuGetLogCodes(warningsNotAsErrors));
        }

        /// <summary>
        /// Create warning properties from the msbuild property strings.
        /// </summary>
        [Obsolete]
        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, string warningsAsErrors, string noWarn)
        {
            return GetWarningProperties(treatWarningsAsErrors, MSBuildStringUtility.GetNuGetLogCodes(warningsAsErrors), MSBuildStringUtility.GetNuGetLogCodes(noWarn));
        }

        /// <summary>
        /// Create warning properties from NuGetLogCode collection.
        /// </summary>
        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, IEnumerable<NuGetLogCode> warningsAsErrors, IEnumerable<NuGetLogCode> noWarn, IEnumerable<NuGetLogCode> warningsNotAsErrors)
        {
            var props = new WarningProperties()
            {
                AllWarningsAsErrors = MSBuildStringUtility.IsTrue(treatWarningsAsErrors)
            };

            props.WarningsAsErrors.UnionWith(warningsAsErrors);
            props.NoWarn.UnionWith(noWarn);
            props.WarningsNotAsErrors.UnionWith(warningsNotAsErrors);
            return props;
        }

        /// <summary>
        /// Create warning properties from NuGetLogCode collection.
        /// </summary>
        [Obsolete]
        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, IEnumerable<NuGetLogCode> warningsAsErrors, IEnumerable<NuGetLogCode> noWarn)
        {
            return GetWarningProperties(treatWarningsAsErrors, warningsAsErrors, noWarn, Enumerable.Empty<NuGetLogCode>());
        }
    }
}
