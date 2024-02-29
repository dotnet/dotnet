// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using NuGet.Protocol;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class RestoreAuditProperties : IEquatable<RestoreAuditProperties>
    {
        /// <summary>
        /// Gets or sets a value indicating whether NuGet audit (check packages for known vulnerabilities) is enabled.
        /// </summary>
        public string? EnableAudit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the NuGet audit level threshold with which vulnerabilities are reported.
        /// </summary>
        /// <value>low, moderate, high, critical</value>
        public string? AuditLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which audit mode to use.
        /// </summary>
        /// <value>direct, all</value>
        public string? AuditMode { get; set; }

        // Enum parsing and ToString are a magnitude of times slower than a naive implementation.
        public bool TryParseEnableAudit(out bool result)
        {
            // Earlier versions allowed "enable" and "default" to opt-in
            if (string.IsNullOrEmpty(EnableAudit)
                || string.Equals(EnableAudit, bool.TrueString, StringComparison.OrdinalIgnoreCase)
                || string.Equals(EnableAudit, "enable", StringComparison.OrdinalIgnoreCase)
                || string.Equals(EnableAudit, "default", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }
            if (string.Equals(EnableAudit, bool.FalseString, StringComparison.OrdinalIgnoreCase)
                || string.Equals(EnableAudit, "disable", StringComparison.OrdinalIgnoreCase))
            {
                result = false;
                return true;
            }
            result = true;

            return false;
        }

        public bool TryParseAuditLevel(out PackageVulnerabilitySeverity result)
        {
            if (AuditLevel == null)
            {
                result = PackageVulnerabilitySeverity.Low;
                return true;
            }

            if (string.Equals(AuditLevel, "low", StringComparison.OrdinalIgnoreCase))
            {
                result = PackageVulnerabilitySeverity.Low;
                return true;
            }
            if (string.Equals(AuditLevel, "moderate", StringComparison.OrdinalIgnoreCase))
            {
                result = PackageVulnerabilitySeverity.Moderate;
                return true;
            }
            if (string.Equals(AuditLevel, "high", StringComparison.OrdinalIgnoreCase))
            {
                result = PackageVulnerabilitySeverity.High;
                return true;
            }
            if (string.Equals(AuditLevel, "critical", StringComparison.OrdinalIgnoreCase))
            {
                result = PackageVulnerabilitySeverity.Critical;
                return true;
            }

            result = PackageVulnerabilitySeverity.Unknown;
            return false;
        }

        public bool Equals(RestoreAuditProperties? other)
        {
            if (other is null) return false;

            return EnableAudit == other.EnableAudit &&
                AuditLevel == other.AuditLevel &&
                AuditMode == other.AuditMode;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RestoreAuditProperties);
        }

        public static bool operator ==(RestoreAuditProperties? x, RestoreAuditProperties? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;

            return x.Equals(y);
        }

        public static bool operator !=(RestoreAuditProperties? x, RestoreAuditProperties? y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            var hashCodeCombiner = new HashCodeCombiner();
            hashCodeCombiner.AddObject(EnableAudit);
            hashCodeCombiner.AddObject(AuditLevel);
            hashCodeCombiner.AddObject(AuditMode);
            return hashCodeCombiner.CombinedHash;
        }

        internal RestoreAuditProperties Clone()
        {
            var clone = new RestoreAuditProperties()
            {
                EnableAudit = EnableAudit,
                AuditLevel = AuditLevel,
                AuditMode = AuditMode,
            };
            return clone;
        }
    }
}
