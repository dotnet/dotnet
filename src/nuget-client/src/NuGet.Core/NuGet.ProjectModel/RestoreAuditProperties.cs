// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
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
