// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public class VerifyArgs
    {
        /// <summary>
        /// Available types of verification.
        /// </summary>
        public enum Verification
        {
            Unknown,
            All,
            Signatures
        };

        /// <summary>
        /// Types of verifications to be performed.
        /// </summary>
        public IList<Verification> Verifications { get; set; }

        /// <summary>
        /// Path to the package that has to be verified.
        /// </summary>      
        [Obsolete("Use PackagePaths instead")]
        public string PackagePath
        {
            get
            {
                switch (PackagePaths.Count)
                {
                    case 0:
                        return null;

                    case 1:
                        return PackagePaths[0];

                    default:
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture,
                            Strings.Error_MultiplePackagePaths,
                            nameof(PackagePaths)));
                }
            }
            set => PackagePaths = new[] { value };
        }

        /// <summary>
        /// Paths to the packages that has to be verified.
        /// </summary>
        public IReadOnlyList<string> PackagePaths { get; set; }

        /// <summary>
        /// Logger to be used to display the logs during the execution of verify command.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Amount of detail the logger should receive
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// If not empty, signer certificate fingerprint must match one in this list
        /// </summary>
        public IEnumerable<string> CertificateFingerprint { get; set; }

        /// <summary>
        /// Loaded NuGet settings
        /// </summary>
        public ISettings Settings { get; set; }
    }
}
