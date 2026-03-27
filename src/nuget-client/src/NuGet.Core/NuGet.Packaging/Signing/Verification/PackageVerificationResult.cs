// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Represents a signature check result and any additional information
    /// needed to display to the user.
    /// </summary>
    public abstract class PackageVerificationResult
    {
        /// <summary>
        /// Trust result
        /// </summary>
        public virtual SignatureVerificationStatus Trust { get; }

        /// <summary>
        /// List of issues found in the verification process
        /// </summary>
        public virtual IEnumerable<SignatureLog> Issues { get; }

        /// <summary>
        /// PackageVerificationResult
        /// </summary>
        public PackageVerificationResult(SignatureVerificationStatus trust, IEnumerable<SignatureLog> issues)
        {
            Trust = trust;
            Issues = issues ?? new List<SignatureLog>();
        }

        public IEnumerable<ILogMessage> GetWarningIssues()
        {
            return Issues.Where(p => p.Level == LogLevel.Warning);
        }

        public IEnumerable<ILogMessage> GetErrorIssues()
        {
            return Issues.Where(p => p.Level == LogLevel.Error);
        }
    }
}
